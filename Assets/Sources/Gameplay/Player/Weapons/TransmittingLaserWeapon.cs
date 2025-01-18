using Assets.Sources.Types;
using UnityEngine;

namespace Assets.Sources.Gameplay.Player.Weapons
{
    public class TransmittingLaserWeapon : PlayerTankWeapon
    {
        protected override void Shoot()
        {
            BulletFactory.CreateHomingBullet(HomingBulletType.Laser, GetBulletPoint(0).position, BulletRotation);
            OnBulletCreated();
        }

        protected override void SuperShoot()
        {
            BulletFactory.CreateTransmittingLaser(GetBulletPoint(0).position, BulletRotation);
            OnBulletCreated();
        }
    }
}
