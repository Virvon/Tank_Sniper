using Assets.Sources.Infrastructure.Factories.BulletFactory;
using Assets.Sources.Types;
using UnityEngine;

namespace Assets.Sources.MainMenu.Weapons
{
    public class ForwardFlyingBulletsWeapon : Weapon
    {
        [SerializeField] private ForwardFlyingBulletType _bulletType;

        protected override void CreateBullet(IBulletFactory bulletFactory, Vector3 position, Quaternion rotation) =>
            bulletFactory.CreateForwardFlyingBullet(_bulletType, position, rotation);
    }
}
