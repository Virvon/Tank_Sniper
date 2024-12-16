using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class PlayerTankWrapper : MonoBehaviour
    {
        private Aiming _aiming;
        private AimingCameraPoint _aimingCameraPoint;
        private AimingConfig _aimingConfig;

        private Transform[] _bulletPoints;
        private Transform _turret;
        private float _movingDistance;
        private Vector3 _startPosition;
        private Quaternion _startTurretRotation;

        private Coroutine _mover;

        [Inject]
        private void Construct(Aiming aiming, AimingCameraPoint aimingCameraPoint, IStaticDataService staticDataService)
        {
            _aiming = aiming;
            _aimingCameraPoint = aimingCameraPoint;
            _aimingConfig = staticDataService.AimingConfig;

            _aiming.StateChanged += OnAimingStateChanged;
        }

        private void OnDestroy()
        {
            _aiming.StateChanged -= OnAimingStateChanged;
        }

        public void Initialize(Transform[] bulletPoints, Transform turret)
        {
            _bulletPoints = bulletPoints;
            _turret = turret;

            _movingDistance = MathF.Abs(_aimingCameraPoint.transform.position.x - turret.position.x);
            _startPosition = transform.position;
            _startTurretRotation = _turret.rotation;
        }

        private void OnAimingStateChanged(bool isAimed, float duration)
        {
            if (_mover != null)
                StopCoroutine(_mover);

            _mover = StartCoroutine(Mover(isAimed, duration));
        }

        private IEnumerator Mover(bool isAimed, float duration)
        {
            float progress;
            float passedTime = 0;
            bool isCompleted = false;

            Vector3 startPosition = transform.position;
            Vector3 targetPosition = isAimed ? _startPosition + transform.forward * _movingDistance * _aimingConfig.TankMovingDistanceModifier : _startPosition;

            Quaternion startRotation = _turret.rotation;
            Quaternion targetRotation = isAimed ? _startTurretRotation * Quaternion.AngleAxis(_aimingConfig.TankTurretRotation, Vector3.up) : _startTurretRotation;

            while(isCompleted == false)
            {
                progress = passedTime / duration;
                passedTime += Time.deltaTime;

                transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
                _turret.rotation = Quaternion.Lerp(startRotation, targetRotation, progress);

                isCompleted = progress >= 1;

                yield return null;
            }
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<PlayerTankWrapper>>
        {
        }
    }
}
