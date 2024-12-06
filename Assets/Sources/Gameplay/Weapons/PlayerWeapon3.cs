using Assets.Sources.Types;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Weapons
{
    public class PlayerWeapon3 : PlayerWeapon
    {
        [SerializeField] private uint _superBulletShootsCount;
        [SerializeField] private float _supperBUlletShootsDuration;

        protected override void Shoot()
        {
            GameplayFactory.CreateLaser(BulletType.Laser, ShootPoint, BulletRotation);
        }

        protected override void SuperShoot()
        {
            StartCoroutine(Shooter());
        }

        private IEnumerator Shooter()
        {
            WaitForSeconds duration = new WaitForSeconds(_supperBUlletShootsDuration);

            for (int i = 0; i < _superBulletShootsCount; i++)
            {
                GameplayFactory.CreateHomingBullet(BulletType.HomingLaser, ShootPoint, BulletRotation);

                yield return duration;
            }
        }
    }
}
