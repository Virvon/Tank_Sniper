using Assets.Sources.Types;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Player.Weapons
{
    public class ForwardFlyingBulletsPlayerTankWeapon : PlayerTankWeapon
    {       
        [SerializeField] private ForwardFlyingBulletType _bulletType;
        [SerializeField] private ForwardFlyingBulletType _supperBulletType;

        protected override void Shoot() =>
            StartCoroutine(Shooter(BulletShootsCount, BulletShootsDuration, _bulletType));

        protected override void SuperShoot() =>
            StartCoroutine(Shooter(SuperBulletShootsCount, SuperBulletShootsDuration, _supperBulletType));

        private IEnumerator Shooter(uint shootsCount, float shootsDuration, ForwardFlyingBulletType bulletType)
        {
            WaitForSeconds duration = new WaitForSeconds(shootsDuration);

            for (int i = 0; i < shootsCount; i++)
            {
                BulletFactory.CreateForwardFlyingBullet(bulletType, GetBulletPoint(i).position, BulletRotation);
                OnBulletCreated();

                yield return duration;
            }
        }
    }
}
