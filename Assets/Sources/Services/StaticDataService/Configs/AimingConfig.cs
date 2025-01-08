using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "AimingConfig", menuName = "Configs/Create new aiming config", order = 51)]
    public class AimingConfig : ScriptableObject
    {
        public float AimingDuration;
        public float ShootingAimDuration;
        public int TankTurretRotation;
        public float TankMovingDistanceModifier;
        public Vector2 StartRotation;
        public Vector2 MaxRotation;
        public Vector2 MinRotation;

    }
}