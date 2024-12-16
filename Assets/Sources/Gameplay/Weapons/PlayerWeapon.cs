using Assets.Sources.Infrastructure.Factories.BulletFactory;
using Assets.Sources.Services.InputService;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Weapons
{
    public abstract class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] private uint _bulletsCapacity;
        [SerializeField] private uint _requireShotsNumberToSuperShot;
        [SerializeField] private Transform _shootPoint;

        private IInputService _inputService;
        private GameplayCamera _gameplayCamera;

        private uint _shootsNumberToSuperShot;
        private uint _bulletsCount;

        protected IBulletFactory BulletFactory { get; private set; }
        protected Vector3 ShootPoint => _shootPoint.position;
        protected Quaternion BulletRotation => _gameplayCamera.transform.rotation;

        [Inject]
        private void Construct(IBulletFactory bulletFactory, IInputService inputService, GameplayCamera gameplayCamera)
        {
            BulletFactory = bulletFactory;
            _inputService = inputService;
            _gameplayCamera = gameplayCamera;

            _shootsNumberToSuperShot = _requireShotsNumberToSuperShot;
            _bulletsCount = _bulletsCapacity;

            //.Shooted += OnShoted;
        }

        private void OnDestroy()
        {
            //_inputService.Shooted -= OnShoted;
        }

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

        protected abstract void Shoot();

        protected abstract void SuperShoot();
    }
}
