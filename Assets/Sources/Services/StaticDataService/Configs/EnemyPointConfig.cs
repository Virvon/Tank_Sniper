using System;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.Building
{
    [Serializable]
    public class EnemyPointConfig
    {
        public string Id;
        public Vector3 Position;
        public Quaternion Rotation;

        public EnemyPointConfig(string id, Vector3 position, Quaternion rotation)
        {
            Id = id;
            Position = position;
            Rotation = rotation;
        }
    }
}