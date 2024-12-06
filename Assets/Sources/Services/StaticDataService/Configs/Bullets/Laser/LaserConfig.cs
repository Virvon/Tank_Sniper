using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.Bullets.Laser
{
    [CreateAssetMenu(fileName = "LaserConfig", menuName = "Configs/Bullets/Create new laser config", order = 51)]
    public class LaserConfig : BulletConfig
    {
        public float ProjectileLifeTime;
    }
}