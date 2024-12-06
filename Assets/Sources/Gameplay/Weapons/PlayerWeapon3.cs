using Assets.Sources.Types;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Weapons
{
    public class PlayerWeapon3 : PlayerWeapon
    {
        [SerializeField] private uint _bulletsShootsCount;
        [SerializeField] private float _bulletShootsDuration;
        [SerializeField] private uint _superBulletShootsCount;
        [SerializeField] private float _supperBulletShootsDuration;

        protected override void Shoot()
        {
            StartCoroutine(Shooter(_bulletsShootsCount, _bulletShootsDuration));
        }

        protected override void SuperShoot()
        {
            StartCoroutine(Shooter(_superBulletShootsCount, _supperBulletShootsDuration));
        }

        private IEnumerator Shooter(uint bulletsCount, float shootsDuration)
        {
            WaitForSeconds duration = new WaitForSeconds(shootsDuration);

            for (int i = 0; i < bulletsCount; i++)
            {
                //GameplayFactory.CreateHomingBullet(bulletType, ShootPoint, BulletRotation);

                yield return duration;
            }
        }
    }
}
