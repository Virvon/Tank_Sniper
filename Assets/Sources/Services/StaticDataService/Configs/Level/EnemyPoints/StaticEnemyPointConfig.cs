using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.Level.EnemyPoints
{
    [Serializable]
    public class StaticEnemyPointConfig
    {
        public string Id;
        public Vector3 StartPosition;
        public Quaternion StartRotation;
        public EnemyType EnemyType;

        public StaticEnemyPointConfig(string id, Vector3 startPosition, Quaternion startRotation, EnemyType enemyType)
        {
            Id = id;
            StartPosition = startPosition;
            StartRotation = startRotation;
            EnemyType = enemyType;
        }

        public virtual async UniTask<Enemy> Create(IGameplayFactory gameplayFactory) =>
            await gameplayFactory.CreateEnemy(EnemyType, StartPosition, StartRotation);
    }
}