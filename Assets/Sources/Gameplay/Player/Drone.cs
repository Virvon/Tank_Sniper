﻿using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.Destructions;
using Assets.Sources.Gameplay.Player.Aiming;
using Cinemachine;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Player
{
    public class Drone : HandleableRotationObject
    {
        private const int ActiveCameraPriority = 1;
        private const int DeactiveCameraPriority = 0;

        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private float _cameraBlendDuration;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed;
        [SerializeField] private DroneExplosion _explosion;
        [SerializeField] private GameObject _drone;
        [SerializeField] private Collider _collider;
        [SerializeField] private uint _maxRotation;
        [SerializeField] private uint _maxDistance;

        private DroneAiming _aiming;
        private GameplayCamera _gameplayCamera;
        private RotationCamera _rotationCamera;

        private bool _canMove;
        private bool _isCollided;
        private bool _isExploded;
        private bool _isShootedProcecc;

        private Vector3 _startDiretion;
        private Vector3 _startPosition;

        public event Action Exploded;

        [Inject]
        private void Construct(DroneAiming droneAiming, GameplayCamera gameplayCamera, RotationCamera rotationCamera)
        {
            _aiming = droneAiming;
            _gameplayCamera = gameplayCamera;
            _rotationCamera = rotationCamera;

            _canMove = false;
            _isCollided = false;
            _isExploded = false;
            _isShootedProcecc = false;

            _aiming.Shooted += OnPlayerShooted;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _aiming.Shooted -= OnPlayerShooted;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_isCollided || _isExploded)
                return;

            _isCollided = true;
            Explode();
        }

        private void Update()
        {
            if (_canMove == false || _isExploded)
                return;

            if (Vector3.Angle(_rigidbody.transform.forward, _startDiretion) > _maxRotation
                || Vector3.Distance(_startPosition, transform.position) > _maxDistance)
                Explode();
        }

        protected override Vector2 ClampRotation(Vector2 rotation) =>
            rotation;

        protected override void OnAimShifted(Vector2 handlePosition)
        {
            if(_canMove)
            {
                base.OnAimShifted(handlePosition);
                _rigidbody.velocity = _rigidbody.transform.forward * _speed;
            }
        }

        protected override void OnHandlePressed(Vector2 handlePosition)
        {
            if(_canMove)
                base.OnHandlePressed(handlePosition);
        }

        private void OnPlayerShooted()
        {
            if (_isShootedProcecc)
                return;

            _isShootedProcecc = true;
            _gameplayCamera.SetBlednDuration(_cameraBlendDuration);
            _camera.Priority = ActiveCameraPriority;

            _startDiretion = _rigidbody.transform.forward;
            _startPosition = transform.position;

            Rotation = _rotationCamera.Rotation;

            StartCoroutine(Rotater());
        }

        private IEnumerator Rotater()
        {
            float progress;
            float passedTime = 0;
            Vector2 targetRotation = _rotationCamera.Rotation;
            Vector2 startRotation = new Vector2(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y);

            while (Rotation != targetRotation)
            {
                progress = passedTime / _cameraBlendDuration;
                passedTime += Time.deltaTime;

                Rotation = Vector2.Lerp(startRotation, targetRotation, progress);
                Rotate();

                yield return null;
            }

            _canMove = true;
            _isShootedProcecc = false;
            _rigidbody.velocity = _rigidbody.transform.forward * _speed;
        }

        private void Explode()
        {
            _isExploded = true;
            _camera.Priority = DeactiveCameraPriority;
            _gameplayCamera.SetBlednDuration(0);
            _rotationCamera.ResetRotation();
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
            _explosion.Explode();
            Destroy(_drone);
            Exploded?.Invoke();
        }

        public class Factory : PlaceholderFactory<string, Vector3, Quaternion, UniTask<Drone>>
        {
        }
    }
}