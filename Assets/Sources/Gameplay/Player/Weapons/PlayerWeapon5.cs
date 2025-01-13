using Assets.Sources.Types;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Player.Weapons
{
    public class PlayerWeapon5 : PlayerTankWeapon
    {
        [SerializeField] private uint _bulletShootsCount;
        [SerializeField] private float _bulletShootsDuration;

        protected override void Shoot()
        {
            StartCoroutine(Shooter(_bulletShootsCount, _bulletShootsDuration));
        }

        protected override void SuperShoot()
        {
            //GameplayFactory.CreateTransmittingLaser(BulletType.TransmittingLaser, ShootPoint, BulletRotation);
        }

        private IEnumerator Shooter(uint shootsCount, float shootsDuration)
        {
            WaitForSeconds duration = new WaitForSeconds(shootsDuration);

            for (int i = 0; i < shootsCount; i++)
            {
                //GameplayFactory.CreateLaser(bulletType, ShootPoint, BulletRotation);

                yield return duration;
            }
        }
    }
}
