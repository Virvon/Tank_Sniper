using Assets.Sources.Types;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Weapons
{
    public class PlayerWeapon2 : PlayerWeapon
    {
        [SerializeField] private uint _superBulletShootsCount;
        [SerializeField] private float _supperBUlletShootsDuration;
        [SerializeField] private BulletType _bulletType;

        protected override void Shoot()
        {
            GameplayFactory.CreateTankRocked(_bulletType, ShootPoint, BulletRotation);
        }

        protected override void SuperShoot() =>
            StartCoroutine(Shooter());

        private IEnumerator Shooter()
        {
            WaitForSeconds duration = new WaitForSeconds(_supperBUlletShootsDuration);

            for(int i = 0; i < _superBulletShootsCount; i++)
            {
                GameplayFactory.CreateTankRocked(_bulletType, ShootPoint, BulletRotation);

                yield return duration;
            }
        }
    }
}
