using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.Player.Aiming;
using Assets.Sources.Infrastructure.Factories.BulletFactory;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Weapons
{
    public abstract class PlayerTankWeapon : MonoBehaviour
    {
        [SerializeField] private uint _bulletsCapacity;
        [SerializeField] private uint _requireShotsNumberToSuperShot;

        private GameplayCamera _gameplayCamera;
        private TankAiming _aiming;

        private Transform[] _bulletPoints;

        private uint _shootsNumberToSuperShot;
        private uint _bulletsCount;

        protected IBulletFactory BulletFactory { get; private set; }
        protected Quaternion BulletRotation => _gameplayCamera.transform.rotation;

        [Inject]
        private void Construct(IBulletFactory bulletFactory, GameplayCamera gameplayCamera, TankAiming aiming)
        {
            BulletFactory = bulletFactory;
            _gameplayCamera = gameplayCamera;
            _aiming = aiming;

            _shootsNumberToSuperShot = _requireShotsNumberToSuperShot;
            _bulletsCount = _bulletsCapacity;

            _aiming.Shooted += OnShoted;
        }

        public void SetBulletPoints(Transform[] bulletPoints) =>
            _bulletPoints = bulletPoints;

        private void OnDestroy() =>
            _aiming.Shooted -= OnShoted;

        private void OnShoted()
        {
            if (_bulletsCount == 0)
                _bulletsCount = _bulletsCapacity;

            _bulletsCount--;

            if(_shootsNumberToSuperShot == 0)
            {
                _shootsNumberToSuperShot = _requireShotsNumberToSuperShot;

                SuperShoot();
            }
            else
            {
                _shootsNumberToSuperShot--;

                Shoot();
            }
        }

        protected Transform GetBulletPoint(int index)
        {
            int bulletPointIndex = index >= _bulletPoints.Length ? _bulletPoints.Length % index : index;

            return _bulletPoints[bulletPointIndex];
        }

        protected abstract void Shoot();

        protected abstract void SuperShoot();
    }
}
