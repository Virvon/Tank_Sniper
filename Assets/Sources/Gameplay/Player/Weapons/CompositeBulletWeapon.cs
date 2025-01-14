using Assets.Sources.Types;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Player.Weapons
{
    public class CompositeBulletWeapon : PlayerTankWeapon
    {
        protected override void Shoot() =>
            StartCoroutine(Shooter(BulletShootsCount, BulletShootsDuration, ForwardFlyingBulletType.Bullet));

        protected override void SuperShoot()
        {
            BulletFactory.CreateCompositeBullet(GetBulletPoint(0).position, BulletRotation);
            OnBulletCreated();
        }

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
