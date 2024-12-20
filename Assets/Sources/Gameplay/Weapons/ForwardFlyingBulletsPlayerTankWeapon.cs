using Assets.Sources.Types;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Weapons
{
    public class ForwardFlyingBulletsPlayerTankWeapon : PlayerTankWeapon
    {
        [SerializeField] private uint _bulletShootsCount;
        [SerializeField] private float _bulletShootsDuration;
        [SerializeField] private ForwardFlyingBulletType _bulletType;

        [SerializeField] private uint _superBulletShootsCount;
        [SerializeField] private float _supperBulletShootsDuration;
        [SerializeField] private ForwardFlyingBulletType _supperBulletType;

        protected override void Shoot() =>
            StartCoroutine(Shooter(_bulletShootsCount, _bulletShootsDuration, _bulletType));

        protected override void SuperShoot() =>
            StartCoroutine(Shooter(_superBulletShootsCount, _supperBulletShootsDuration, _supperBulletType));

        private IEnumerator Shooter(uint shootsCount, float shootsDuration, ForwardFlyingBulletType bulletType)
        {
            WaitForSeconds duration = new WaitForSeconds(shootsDuration);

            for(int i = 0; i < shootsCount; i++)
            {
                BulletFactory.CreateForwardFlyingBullet(bulletType, GetBulletPoint(i).position, BulletRotation);

                yield return duration;
            }
        }
    }
}
