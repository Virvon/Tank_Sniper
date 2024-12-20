using Assets.Sources.Types;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Weapons
{
    public class PlayerWeapon4 : PlayerTankWeapon
    {
        [SerializeField] private uint _bulletShootsCount;
        [SerializeField] private float _bulletShootsDuration;
        [SerializeField] private uint _superBulletShootsCount;
        [SerializeField] private float _supperBulletShootsDuration;

        protected override void Shoot()
        {
            StartCoroutine(Shooter(_bulletShootsCount, _bulletShootsDuration));
        }

        protected override void SuperShoot()
        {
            StartCoroutine(Shooter(_superBulletShootsCount, _supperBulletShootsDuration));
        }

        private IEnumerator Shooter(uint shootsCount, float shootsDuration)
        {
            WaitForSeconds duration = new WaitForSeconds(shootsDuration);

            for(int i = 0; i < shootsCount; i++)
            {
                //GameplayFactory.CreateTankRocked(bulletType, ShootPoint, BulletRotation);

                yield return duration;
            }
        }
    }
}
