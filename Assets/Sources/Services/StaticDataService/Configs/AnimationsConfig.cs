using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "AnimationsConfig", menuName = "Configs/Create new animations config", order = 51)]
    public class AnimationsConfig : ScriptableObject
    {
        public AnimationCurve TankShootingAnimationCurve;
        public uint TankShootingRotationAngle;
        public float TankShootingDuration;
    }
}