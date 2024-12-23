﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.Level
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/Create new level config", order = 51)]
    public class LevelConfig : ScriptableObject, IConfig<string>
    {
        public string LevelKey;
        public List<StaticEnemyPointConfig> StaticEnemyPoints;
        //public List<WalkingEnemyPointConfig> WalkingEnemyPoints;
        //public List<EnemyCarPointConfig> EnemyCarPoints;
        public List<EnemyCarPointConfig> EnemyCarPoints;
        public List<HelicopterPointConfig> HelicopterPoints;

        public string Key => LevelKey;
    }
}