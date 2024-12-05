namespace Assets.Sources.Gameplay.Weapons
{
    public class PlayerWeapon1 : PlayerWeapon
    {
        protected override void Shoot()
        {
            GameplayFactory.CreateTankRocked(Types.BulletType.TankRocket, ShootPoint, BulletRotation);
        }

        protected override void SuperShoot()
        {
            GameplayFactory.CreateTankRocked(Types.BulletType.SuperTankRocket, ShootPoint, BulletRotation);
        }
    }
}
