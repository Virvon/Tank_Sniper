using Assets.Sources.Infrastructure.Factories.BulletFactory;
using Assets.Sources.Types;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.MainMenu.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] private uint _bulletsCount;
        [SerializeField] private float _shootsCooldown;
        [SerializeField] private MuzzleType _muzzleType;

        private Transform[] _bulletPoints;

        private IBulletFactory _bulletFactory;

        public bool IsShooted { get; private set; }

        [Inject]
        private void Construct(IBulletFactory bulletFactory)
        {
            _bulletFactory = bulletFactory;

            IsShooted = false;
        }

        public void SetBulletPoints(Transform[] bulletPoints) =>
            _bulletPoints = bulletPoints;

        public void Shoot(Action shooted) =>
            StartCoroutine(Shooter(shooted));

        protected abstract void CreateBullet(IBulletFactory bulletFactory, Vector3 position, Quaternion rotation);

        private IEnumerator Shooter(Action shooted)
        {
            WaitForSeconds cooldown = new WaitForSeconds(_shootsCooldown);
            int bulletPointIndex = 0;

            IsShooted = true;

            for (int i = 0; i < _bulletsCount; i++)
            {
                bulletPointIndex = bulletPointIndex >= _bulletPoints.Length ? 0 : bulletPointIndex;
                Transform bulletPoint = _bulletPoints[bulletPointIndex];

                shooted?.Invoke();

                _bulletFactory.CreateMuzzle(_muzzleType, bulletPoint.position, bulletPoint.rotation);
                CreateBullet(_bulletFactory, bulletPoint.position, bulletPoint.rotation);

                bulletPointIndex++;

                yield return cooldown;
            }

            IsShooted = false;
        }
    }
}
