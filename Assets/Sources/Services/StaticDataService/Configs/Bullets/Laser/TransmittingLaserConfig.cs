using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.Bullets.Laser
{
    [CreateAssetMenu(fileName = "TransmittingLaserConfig", menuName = "Configs/Bullets/Create new transmitting laser config", order = 51)]
    public class TransmittingLaserConfig : LaserConfig
    {
        public int TargetsCount;
    }
}