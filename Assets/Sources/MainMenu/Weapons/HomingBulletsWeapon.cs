using Assets.Sources.Infrastructure.Factories.BulletFactory;
using Assets.Sources.Types;
using UnityEngine;

namespace Assets.Sources.MainMenu.Weapons
{
    public class HomingBulletsWeapon : Weapon
    {
        protected override void CreateBullet(IBulletFactory bulletFactory, Vector3 position, Quaternion rotation) =>
            bulletFactory.CreateHomingBullet(HomingBulletType.Laser, position, rotation);
    }
}
