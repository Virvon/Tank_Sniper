﻿using System;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.Level
{
    [Serializable]
    public class EnemyPointConfig
    {
        public string Id;
        public Vector3 Position;
        public Quaternion Rotation;
        public EnemyType EnemyType;

        public EnemyPointConfig(string id, Vector3 position, Quaternion rotation, EnemyType enemyType)
        {
            Id = id;
            Position = position;
            Rotation = rotation;
            EnemyType = enemyType;
        }
    }
}