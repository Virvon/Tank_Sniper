using Assets.Sources.Types;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Player.Weapons
{
    public class DirectionalLaserWeapon : PlayerTankWeapon
    {
        protected override void Shoot()
        {
            BulletFactory.CreateDirectionalLaser(GetBulletPoint(0).position, BulletRotation);
            OnBulletCreated();
        }

        protected override void SuperShoot()
        {
            StartCoroutine(Shooter(SuperBulletShootsCount, SuperBulletShootsDuration));
        }

        private IEnumerator Shooter(uint shootsCount, float shootsDuration)
        {
            WaitForSeconds duration = new WaitForSeconds(shootsDuration);

            for (int i = 0; i < shootsCount; i++)
            {
                BulletFactory.CreateHomingBullet(HomingBulletType.Laser, GetBulletPoint(i).position, BulletRotation);
                OnBulletCreated();

                yield return duration;
            }
        }
    }
}
