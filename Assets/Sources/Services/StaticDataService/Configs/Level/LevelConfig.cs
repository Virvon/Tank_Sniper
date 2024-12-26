using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.Level
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/Create new level config", order = 51)]
    public class LevelConfig : ScriptableObject, IConfig<string>
    {
        public string LevelKey;
        public List<StaticEnemyPointConfig> StaticEnemyPoints;
        public List<PatrolingEnemyPointConfig> PatrolingEnemyPoints;
        public List<EnemyCarPointConfig> EnemyCarPoints;

        public string Key => LevelKey;
    }
}