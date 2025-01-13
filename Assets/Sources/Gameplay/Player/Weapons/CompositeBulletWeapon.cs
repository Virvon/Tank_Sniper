using Assets.Sources.Types;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Player.Weapons
{
    public class CompositeBulletWeapon : PlayerTankWeapon
    {
        protected override void Shoot()
        {
            StartCoroutine(Shooter(BulletsCount, BulletShootsDuration, ForwardFlyingBulletType.Bullet));
        }

        protected override void SuperShoot()
        {
            BulletFactory.CreateCompositeBullet(GetBulletPoint(0).position, BulletRotation);
        }

        private IEnumerator Shooter(uint shootsCount, float shootsDuration, ForwardFlyingBulletType bulletType)
        {
            WaitForSeconds duration = new WaitForSeconds(shootsDuration);

            for (int i = 0; i < shootsCount; i++)
            {
                BulletFactory.CreateForwardFlyingBullet(bulletType, GetBulletPoint(i).position, BulletRotation);

                yield return duration;
            }
        }
    }
}
