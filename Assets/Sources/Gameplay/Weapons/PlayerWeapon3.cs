using Assets.Sources.Types;

namespace Assets.Sources.Gameplay.Weapons
{
    public class PlayerWeapon3 : PlayerWeapon
    {
        protected override void Shoot()
        {
            GameplayFactory.CreateLaser(BulletType.Laser, ShootPoint, BulletRotation);
        }

        protected override void SuperShoot()
        {
            
        }
    }
}
