using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Gameplay.Enemies.Movement;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.Level
{
    [Serializable]
    public class PatrolingEnemyPointConfig : StaticEnemyPointConfig
    {
        public PathPointConfig[] Path;
        public float Speed;
        public uint MaxRotationAngle;

        public PatrolingEnemyPointConfig(
            string id,
            Vector3 startPosition,
            Quaternion startRotation,
            EnemyType enemyType,
            PathPointConfig[] path)
            : base(id, startPosition, startRotation, enemyType)
        {
            Path = path;
        }

        public override async UniTask<Enemy> Create(IGameplayFactory gameplayFactory)
        {
            Enemy enemy = await base.Create(gameplayFactory);

            enemy.GetComponent<EnemyPatroling>().Initialize(Path, Speed, MaxRotationAngle);

            return enemy;
        }
    }
}