namespace Assets.Sources.Gameplay.Player.Weapons
{
    public class TransmittingLaserWeapon : PlayerTankWeapon
    {
        protected override void Shoot()
        {
            BulletFactory.CreateDirectionalLaser(GetBulletPoint(0).position, BulletRotation);
            OnBulletCreated();
        }

        protected override void SuperShoot()
        {
            BulletFactory.CreateTransmittingLaser(GetBulletPoint(0).position, BulletRotation);
            OnBulletCreated();
        }
    }
}
