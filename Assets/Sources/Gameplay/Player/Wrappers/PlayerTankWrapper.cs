using Assets.Sources.Gameplay.Bullets;
using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.Handlers;
using Assets.Sources.Gameplay.Player.Aiming;
using Assets.Sources.Gameplay.Player.Weapons;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using Random = UnityEngine.Random;

namespace Assets.Sources.Gameplay.Player.Wrappers
{
    public class PlayerTankWrapper : PlayerWrapper, IDamageable
    {
        [SerializeField] private PlayerTankWeapon _playerTankWeapon;
        [SerializeField] private GameObject _smokeParticlePrefab;

        private TankAiming _aiming;
        private AimingCameraPoint _aimingCameraPoint;
        private AimingConfig _aimingConfig;
        private DefeatHandler _defeatHandler;
        private GameplayCamera _gameplayCamera;

        private Transform _turret;
        private float _movingDistance;
        private Vector3 _startPosition;
        private Quaternion _startTurretRotation;
        private bool _isDestructed;

        private Coroutine _mover;
        private GameObject _smokeParticle;

        public event Action HealthChanged;
        public event Action<Vector2> Attacked;

        public uint MaxHealth { get; private set; }
        public uint Health { get; private set; }

        [Inject]
        private void Construct(
            TankAiming aiming,
            AimingCameraPoint aimingCameraPoint,
            IStaticDataService staticDataService,
            DefeatHandler defeatHandler,
            GameplayCamera gameplayCamera)
        {
            _aiming = aiming;
            _aimingCameraPoint = aimingCameraPoint;
            _aimingConfig = staticDataService.AimingConfig;
            _defeatHandler = defeatHandler;
            _gameplayCamera = gameplayCamera;

            MaxHealth = staticDataService.GameplaySettingsConfig.PlayerHealth;
            Health = MaxHealth;

            _isDestructed = false;

            _aiming.StateChanged += OnAimingStateChanged;
            _defeatHandler.ProgressRecovered += OnProgressRecovery;
        }

        private void OnDestroy()
        {
            _aiming.StateChanged -= OnAimingStateChanged;
            _defeatHandler.ProgressRecovered -= OnProgressRecovery;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.TryGetComponent(out ExplodingBullet bullet))
            {
                Vector3 bulletPosition = _gameplayCamera.Camera.WorldToScreenPoint(new Vector3(bullet.StartPosition.x, bullet.StartPosition.y, 1));

                Attacked?.Invoke(bulletPosition);
            }
        }

        public void Initialize(Transform[] bulletPoints, Transform turret)
        {
            _turret = turret;

            _movingDistance = MathF.Abs(_aimingCameraPoint.transform.position.x - turret.position.x);
            _startPosition = transform.position;
            _startTurretRotation = _turret.rotation;

            _playerTankWeapon.SetBulletPoints(bulletPoints);
        }

        public void TakeDamage(ExplosionInfo explosionInfo)
        {
            uint damage = explosionInfo.Damage > Health ? Health : explosionInfo.Damage;
            Health -= damage;

            HealthChanged?.Invoke();

            if (Health == 0)
                Destruct();
        }

        private void Destruct()
        {
            if (_isDestructed)
                return;

            _isDestructed = true;

            _smokeParticle = Instantiate(_smokeParticlePrefab, transform);
            _defeatHandler.OnDefeat();
        }

        private void OnProgressRecovery()
        {
            Health = MaxHealth;
            HealthChanged?.Invoke();
            Destroy(_smokeParticle);
        }

        private void OnAimingStateChanged(bool isAimed, float duration)
        {
            if (_mover != null)
                StopCoroutine(_mover);

            _mover = StartCoroutine(Mover(isAimed, duration));
        }

        private IEnumerator Mover(bool isAimed, float duration)
        {
            if (duration == 0)
                yield break;

            float progress;
            float passedTime = 0;
            bool isCompleted = false;

            Vector3 startPosition = transform.position;
            Vector3 targetPosition = isAimed ? _startPosition + transform.forward * _movingDistance * _aimingConfig.TankMovingDistanceModifier : _startPosition;

            Quaternion startRotation = _turret.rotation;
            Quaternion targetRotation = isAimed ? _startTurretRotation * Quaternion.AngleAxis(_aimingConfig.TankTurretRotation, Vector3.up) : _startTurretRotation;

            while (isCompleted == false)
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
