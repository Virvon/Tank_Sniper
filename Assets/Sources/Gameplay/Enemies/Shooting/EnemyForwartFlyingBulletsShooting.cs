using Assets.Sources.Gameplay.Player.Wrappers;
using Assets.Sources.Infrastructure.Factories.BulletFactory;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Types;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Weapons
{
    public abstract class EnemyForwartFlyingBulletsShooting : EnemyShooting
    {
        protected const int AngleDelta = 3;

        private const int RayCastDistance = 300;

        [SerializeField] private float _reloadDuration;
        [SerializeField] private float _shootCooldown;
        [SerializeField] private ForwardFlyingBulletType _bulletType;
        [SerializeField] private uint _bulletsCapacity;
        [SerializeField] private MuzzleType _muzzleType;
        
        private IBulletFactory _bulletFactory;
        private LayerMask _layerMask;

        private uint _bulletsCount;
        private Vector3 _currentShootingPosition;
        private Quaternion _shootingRotation;

        protected override bool CanShoot => CheckPlayerTankVisibility() && base.CanShoot;
        protected abstract Vector3 LookStartPosition { get; }

        [Inject]
        private void Construct(IBulletFactory bulletFactory, IStaticDataService staticDataService)
        {
            _bulletFactory = bulletFactory;
            _layerMask = staticDataService.GameplaySettingsConfig.EnemyLayerMask;

            _bulletsCount = _bulletsCapacity;
        }

        public bool CheckPlayerTankVisibility()
        {
            return Physics.Raycast(LookStartPosition, (PlayerWrapper.transform.position - LookStartPosition).normalized, out RaycastHit hitInfo, RayCastDistance, _layerMask)
                && hitInfo.transform.TryGetComponent(out PlayerWrapper _);
        }

        protected override Vector3 GetCurrentShootingPosition() =>
            _currentShootingPosition;

        protected void CreateBullet()
        {
            OnShooted();
            _bulletFactory.CreateForwardFlyingBullet(_bulletType, _currentShootingPosition, _shootingRotation);
        }

        protected virtual void Shoot() =>
            CreateBullet();

        protected abstract Vector3 GetCurrentShootPointPosition();

        protected override IEnumerator Shooter()
        {
            WaitForSeconds reloadDuration = new WaitForSeconds(_reloadDuration);
            WaitForSeconds shootCooldown = new WaitForSeconds(_shootCooldown);

            while (IsShooted)
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