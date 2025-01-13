using Assets.Sources.Infrastructure.Factories.BulletFactory;
using UnityEngine;

namespace Assets.Sources.MainMenu.Weapons
{
    public class DirectionLaserWeapon : Weapon
    {
        protected override void CreateBullet(IBulletFactory bulletFactory, Vector3 position, Quaternion rotation) =>
            bulletFactory.CreateDirectionalLaser(position, rotation);
    }
}
