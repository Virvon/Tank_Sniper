using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.Destructions;
using Assets.Sources.Gameplay.Player.Aiming;
using Cinemachine;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Assets.Sources.Gameplay.Player
{
    public class Drone : HandleableRotationObject
    {
        private const string MixerVolume = "Volume";
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
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private float _soundVolume;

        private DroneAiming _aiming;
        private GameplayCamera _gameplayCamera;
        private RotationCamera _rotationCamera;
        private CameraNoise _cameraNoise;

        private bool _canMove;
        private bool _isCollided;
        private bool _isExploded;
        private bool _isShootedProcess;

        private Vector3 _startDiretion;
        private Vector3 _startPosition;

        private float _startSoundVolume;

        public event Action Exploded;

        [Inject]
        private void Construct(DroneAiming droneAiming, GameplayCamera gameplayCamera, RotationCamera rotationCamera, CameraNoise cameraNoise)
        {
            _aiming = droneAiming;
            _gameplayCamera = gameplayCamera;
            _rotationCamera = rotationCamera;
            _cameraNoise = cameraNoise;

            _canMove = false;
            _isCollided = false;
            _isExploded = false;
            _isShootedProcess = false;

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

        private void OnPlayerShooted()
        {
            if (_isShootedProcess)
                return;

            _isShootedProcess = true;
            _gameplayCamera.SetBlednDuration(_cameraBlendDuration);
            _camera.Priority = ActiveCameraPriority;
            _cameraNoise.SetActive(true);

            _startDiretion = _rigidbody.transform.forward;
            _startPosition = transform.position;

            _audioMixer.GetFloat(MixerVolume, out _startSoundVolume);
            _audioMixer.SetFloat(MixerVolume, _soundVolume);

            Rotation = new Vector2(0, _rotationCamera.Rotation.y);

            StartCoroutine(Rotater());
        }

        private void Explode()
        {
            _isExploded = true;
            _cameraNoise.SetActive(false);
            _camera.Priority = DeactiveCameraPriority;
            _audioMixer.SetFloat(MixerVolume, _startSoundVolume);
            _gameplayCamera.SetBlednDuration(0);
            _rotationCamera.ResetRotation();
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
            _explosion.Explode();
            Destroy(_drone);
            Exploded?.Invoke();
        }

        private IEnumerator Rotater()
        {
            float progress;
            float passedTime = 0;
            Vector2 targetRotation = _rotationCamera.Rotation;
            Vector2 startRotation = Rotation;

            while (Rotation != targetRotation)
            {
                progress = passedTime / _cameraBlendDuration;
                passedTime += Time.deltaTime;

                Rotation = Vector2.Lerp(startRotation, targetRotation, progress);
                Rotate();

                yield return null;
            }

            _canMove = true;
            _isShootedProcess = false;
            _rigidbody.velocity = _rigidbody.transform.forward * _speed;
        }

        public class Factory : PlaceholderFactory<string, Vector3, Quaternion, UniTask<Drone>>
        {
        }
    }
}