using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.Building
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/Create new level config", order = 51)]
    public class LevelConfig : ScriptableObject
    {
        public string LevelKey;
        public List<EnemyPointConfig> EnemyPoints;
    }
}