using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "GameplaySettingsConfig", menuName = "Configs/Create new gameplay settings config", order = 51)]
    public class GameplaySettingsConfig : ScriptableObject
    {
        public float EnemyScatter;
        public LayerMask EnemyLayerMask;
        public uint PlayerHealth;
        public float WindowsSwitchDeley;
        public float ProgressRecoveryAvailableTime;
        public float ReloadDuration;
        public float PlayerShootCooldown;
    }
}