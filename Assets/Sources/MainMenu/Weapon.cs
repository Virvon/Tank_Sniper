using Assets.Sources.Infrastructure.Factories.BulletFactory;
using Assets.Sources.Types;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.MainMenu
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private uint _bulletsCount;
        [SerializeField] private float _shootsCooldown;
        [SerializeField] private MuzzleType _muzzleType;

        private IBulletFactory _bulletFactory;

        private Transform[] _bulletPoints;

        public bool IsShooted { get; private set; }

        [Inject]
        private void Construct(IBulletFactory bulletFactory)
        {
            _bulletFactory = bulletFactory;

            IsShooted = false;
        }

        public void SetBulletPoints(Transform[] bulletPoints)
        {
            _bulletPoints = bulletPoints;
        }

        public void Shoot(Action shooted)
        {
            StartCoroutine(Shooter(shooted));
        }

        private IEnumerator Shooter(Action shooted)
        {
            WaitForSeconds cooldown = new WaitForSeconds(_shootsCooldown);

            IsShooted = true;

            for(int i = 0; i < _bulletsCount; i++)
            {
                int bulletPointIndex = i >= _bulletPoints.Length ? _bulletPoints.Length % i : i;
                Transform bulletPoint = _bulletPoints[bulletPointIndex];

                shooted?.Invoke();

                _bulletFactory.CreateMuzzle(_muzzleType, bulletPoint.position, bulletPoint.rotation);
                _bulletFactory.CreateForwardFlyingBullet(ForwardFlyingBulletType.SuperBullet, bulletPoint.position, bulletPoint.rotation);

                yield return cooldown;
            }

            IsShooted = false;
        }
    }
}
