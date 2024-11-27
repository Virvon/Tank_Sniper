using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Cysharp.Threading.Tasks;
using System;
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

        public virtual async UniTask<Enemy> Create(IGameplayFactory gameplayFactory)
        {
            return await gameplayFactory.CreateEnemy(EnemyType, Position, Rotation);
        }
    }
}