using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.Bullets.Colliding
{
    [CreateAssetMenu(fileName = "CompositeBulletConfig", menuName = "Configs/Bullets/Create new composite bullet config", order = 51)]

    public class CompositeBulletConfig : CollidingBulletConfig
    {
        public uint BombsCount;
    }
}