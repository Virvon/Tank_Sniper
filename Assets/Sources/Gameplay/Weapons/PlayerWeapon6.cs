using Assets.Sources.Types;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Weapons
{
    public class PlayerWeapon6 : PlayerTankWeapon
    {
        protected override void Shoot()
        {
            StartCoroutine(Shooter());
        }

        protected override void SuperShoot()
        {
            //GameplayFactory.CreateCompositeBullet(BulletType.CompositeBullet, ShootPoint, BulletRotation);
        }

        private IEnumerator Shooter()
        {
            WaitForSeconds duration = new WaitForSeconds(0.2f);

            for (int i = 0; i < 2; i++)
            {
                //GameplayFactory.CreateTankRocked(BulletType.TankRocket, ShootPoint, BulletRotation);

                yield return duration;
            }
        }
    }
}
