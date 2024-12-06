using Assets.Sources.Types;

namespace Assets.Sources.Gameplay.Weapons
{
    public class PlayerWeapon1 : PlayerWeapon
    {
        protected override void Shoot()
        {
            BulletFactory.CreateForwardFlyingBullet(ForwardFlyingBulletType.Bullet, ShootPoint, BulletRotation);
        }

        protected override void SuperShoot()
        {
            BulletFactory.CreateForwardFlyingBullet(ForwardFlyingBulletType.SuperBullet, ShootPoint, BulletRotation);
        }
    }
}
