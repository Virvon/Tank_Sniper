using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.Player.Aiming;
using Assets.Sources.Infrastructure.Factories.BulletFactory;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Player.Weapons
{
    public abstract class PlayerTankWeapon : MonoBehaviour, IShootable
    {
        [SerializeField] private uint _bulletsCapacity;
        [SerializeField] private uint _requireShotsNumberToSuperShot;
        [SerializeField] private uint _bulletShootsCount;
        [SerializeField] private float _bulletShootsDuration;
        [SerializeField] private uint _superBulletShootsCount;
        [SerializeField] private float _supperBulletShootsDuration;

        private GameplayCamera _gameplayCamera;
        private TankAiming _aiming;
        private CameraShaking _cameraShaking;

        private Transform[] _bulletPoints;

        private uint _shootsNumberToSuperShot;
        private uint _bulletsCount;

        public event Action BulletsCountChanged;
        public event Action BulletCreated;

        protected IBulletFactory BulletFactory { get; private set; }
        protected Quaternion BulletRotation => _gameplayCamera.transform.rotation;
        public uint BulletsCount => _bulletsCount;
        public uint RequireShotsNumberToSuperShot => _requireShotsNumberToSuperShot;
        public uint ShootsNumberToSuperShot => _shootsNumberToSuperShot;

        protected uint BulletShootsCount => _bulletShootsCount;
        protected float BulletShootsDuration => _bulletShootsDuration;
        protected uint SuperBulletShootsCount => _superBulletShootsCount;
        protected float SuperBulletShootsDuration => _supperBulletShootsDuration;

        [Inject]
        private void Construct(IBulletFactory bulletFactory, GameplayCamera gameplayCamera, TankAiming aiming, CameraShaking cameraShaking)
        {
            BulletFactory = bulletFactory;
            _gameplayCamera = gameplayCamera;
            _aiming = aiming;
            _cameraShaking = cameraShaking;

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

            if (_shootsNumberToSuperShot == 0)
            {
                _shootsNumberToSuperShot = _requireShotsNumberToSuperShot;

                SuperShoot();
            }
            else
            {
                _shootsNumberToSuperShot--;

                Shoot();
            }

            

            BulletsCountChanged?.Invoke();
        }

        protected void OnBulletCreated()
        {
            _cameraShaking.Shake();
            BulletCreated?.Invoke();
        }

        protected Transform GetBulletPoint(int index)
        {
            int bulletPointIndex = index >= _bulletPoints.Length ? index - ((int)(index / _bulletPoints.Length) * _bulletPoints.Length) : index;

            return _bulletPoints[bulletPointIndex];
        }

        protected abstract void Shoot();

        protected abstract void SuperShoot();
    }
}
