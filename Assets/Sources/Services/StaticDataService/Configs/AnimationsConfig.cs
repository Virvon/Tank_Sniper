using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "AnimationsConfig", menuName = "Configs/Create new animations config", order = 51)]
    public class AnimationsConfig : ScriptableObject
    {
        public AnimationCurve TankShootingAnimationCurve;
        public uint TankShootingRotationAngle;
        public float TankShootingDuration;

        public AnimationCurve TankScalingAnimationCurve;
        public float TankScalingDuration;

        public AnimationCurve DecalScalingAnimationCurve;
        public float DecalScalingDuration;

        public AnimationCurve DroneDefeatRecoveryProgressButtonAnimationCurve;
        public float DroneDefeateRecoveryProgressButtonAnimationDuration;

        public float DroneAnimationRadius;
        public float DroneAnimationSpeed;

        public AnimationCurve TankAimCellAnimationCurve;
        public float TankAimCellAnimationDuration;

        public AnimationCurve RouletteAnimationCurve;
        public float RouletteRotateDuration;

        public float WalletValueChangingDuration;
    }
}