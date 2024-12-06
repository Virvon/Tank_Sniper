using Assets.Sources.Types;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Weapons
{
    public class PlayerWeapon3 : PlayerWeapon
    {
        [SerializeField] private uint _bulletsShootsCount;
        [SerializeField] private float _bulletShootsDuration;
        [SerializeField] private BulletType _bulletType;
        [SerializeField] private uint _superBulletShootsCount;
        [SerializeField] private float _supperBulletShootsDuration;
        [SerializeField] private BulletType _supperBulletType;

        protected override void Shoot()
        {
            StartCoroutine(Shooter(_bulletType, _bulletsShootsCount, _bulletShootsDuration));
        }

        protected override void SuperShoot()
        {
            StartCoroutine(Shooter(_supperBulletType, _superBulletShootsCount, _supperBulletShootsDuration));
        }

        private IEnumerator Shooter(BulletType bulletType, uint bulletsCount, float shootsDuration)
        {
            WaitForSeconds duration = new WaitForSeconds(shootsDuration);

            for (int i = 0; i < bulletsCount; i++)
            {
                GameplayFactory.CreateHomingBullet(bulletType, ShootPoint, BulletRotation);

                yield return duration;
            }
        }
    }
}
