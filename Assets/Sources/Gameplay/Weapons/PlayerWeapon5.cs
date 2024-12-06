using Assets.Sources.Types;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Weapons
{
    public class PlayerWeapon5 : PlayerWeapon
    {
        [SerializeField] private uint _bulletShootsCount;
        [SerializeField] private float _bulletShootsDuration;

        protected override void Shoot()
        {
            StartCoroutine(Shooter(BulletType.Laser, _bulletShootsCount, _bulletShootsDuration));
        }

        protected override void SuperShoot()
        {
            GameplayFactory.CreateTransmittingLaser(BulletType.TransmittingLaser, ShootPoint, BulletRotation);
        }

        private IEnumerator Shooter(BulletType bulletType, uint shootsCount, float shootsDuration)
        {
            WaitForSeconds duration = new WaitForSeconds(shootsDuration);

            for (int i = 0; i < shootsCount; i++)
            {
                GameplayFactory.CreateLaser(bulletType, ShootPoint, BulletRotation);

                yield return duration;
            }
        }
    }
}
