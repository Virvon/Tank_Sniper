using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Gameplay.Player;
using Assets.Sources.Infrastructure.Factories.BulletFactory;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Types;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Weapons
{
    public abstract class EnemyShooting : MonoBehaviour
    {
        protected const int AngleDelta = 3;

        private const int RayCastDistance = 300;

        private readonly Vector3 TargetOffset = new Vector3(0, 2, 0);

        [SerializeField] private float _reloadDuration;
        [SerializeField] private float _shootCooldown;
        [SerializeField] private ForwardFlyingBulletType _bulletType;
        [SerializeField] private uint _bulletsCapacity;
        [SerializeField] private MuzzleType _muzzleType;
        [SerializeField] private Enemy _enemy;

        private Aiming _aiming;
        private IBulletFactory _bulletFactory;
        private EnemiesSettingsConfig _enemiesSettings;

        private bool _isStartedShoot;
        private uint _bulletsCount;
        private bool _isShooted;
        private Vector3 _currentShootingPosition;
        private Quaternion _shootingRotation;

        protected PlayerTankWrapper PlayerTankWrapper { get; private set; }
        protected virtual bool CanShoot => CheckPlayerTankVisibility();
        protected abstract Vector3 LookStartPosition { get; }

        [Inject]
        private void Construct(
            PlayerTankWrapper playerTankWrapper,
            Aiming aiming,
            IBulletFactory bulletFactory,
            IStaticDataService staticDataService)
        {
            PlayerTankWrapper = playerTankWrapper;
            _aiming = aiming;
            _bulletFactory = bulletFactory;
            _enemiesSettings = staticDataService.EnemiesSettingsConfig;

            _isStartedShoot = false;
            _bulletsCount = _bulletsCapacity;
            _isShooted = false;

            _aiming.Shooted += OnPlayerTankAttacked;  
            _enemy.Destructed += OnEnemyDestructed;
        }

        private void OnDestroy()
        {
            _aiming.Shooted -= OnPlayerTankAttacked;
            _enemy.Destructed -= OnEnemyDestructed;
        }

        public bool CheckPlayerTankVisibility()
        {
            return Physics.Raycast(LookStartPosition, (PlayerTankWrapper.transform.position - LookStartPosition).normalized, out RaycastHit hitInfo, RayCastDistance, _enemiesSettings.LayerMask)
                && hitInfo.transform.TryGetComponent(out PlayerTankWrapper _);
        }

        protected virtual void StartShooting() =>
            StartCoroutine(Shooter());

        protected virtual void OnEnemyDestructed() =>
            _isShooted = false;

        protected void CreateBullet() =>
            _bulletFactory.CreateForwardFlyingBullet(_bulletType, _currentShootingPosition, _shootingRotation);

        protected virtual void Shoot() =>
            CreateBullet();

        protected abstract Vector3 GetCurrentShootPointPosition();

        private Quaternion GetShootingRotation()
        {
            Vector2 randomOffset = Random.insideUnitCircle * _enemiesSettings.Scatter;
            Vector3 targetPosition = PlayerTankWrapper.transform.position + TargetOffset + new Vector3(randomOffset.x, randomOffset.y, 0);

            return Quaternion.LookRotation((targetPosition - _currentShootingPosition).normalized);
        }

        private void OnPlayerTankAttacked()
        {
            if (_isStartedShoot)
                return;

            _isStartedShoot = true;

            StartShooting();
        }

        private IEnumerator Shooter()
        {
            _isShooted = true;
            WaitForSeconds reloadDuration = new WaitForSeconds(_reloadDuration);
            WaitForSeconds shootCooldown = new WaitForSeconds(_shootCooldown);

            while (_isShooted)
            {
                yield return new WaitWhile(() => CanShoot == false);

                _currentShootingPosition = GetCurrentShootPointPosition();
                _shootingRotation = GetShootingRotation();
                Shoot();
                _bulletFactory.CreateMuzzle(_muzzleType, _currentShootingPosition, _shootingRotation);
                _bulletsCount--;

                if (_bulletsCount == 0)
                {
                    _bulletsCount = _bulletsCapacity;
                    yield return reloadDuration;
                }
                else
                {
                    yield return shootCooldown;
                }
            }
        }
    }
}