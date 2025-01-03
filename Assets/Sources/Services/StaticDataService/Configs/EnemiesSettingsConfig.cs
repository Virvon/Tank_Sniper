using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "EnemiesSettingsConfig", menuName = "Configs/Create new enemies settings config", order = 51)]
    public class EnemiesSettingsConfig : ScriptableObject
    {
        public float Scatter;
        public LayerMask LayerMask;
    }
}