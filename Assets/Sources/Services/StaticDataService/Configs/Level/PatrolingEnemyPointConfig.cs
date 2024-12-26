using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Gameplay.Enemies.Movement;
using Assets.Sources.Gameplay.Enemies.Points;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using System.Linq;

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
            EnemyPathPoint[] path)
            : base(id, startPosition, startRotation, enemyType)
        {
            Path = path.Select(value => new PathPointConfig(value.transform.position, value.RotationAngle, value.RotationDelta)).ToArray();
        }

        public override async UniTask<Enemy> Create(IGameplayFactory gameplayFactory)
        {
            Enemy enemy = await base.Create(gameplayFactory);

            if(enemy.TryGetComponent(out EnemyMovement enemyMovement))
                enemyMovement.Initialize(Path, Speed, MaxRotationAngle);

            return enemy;
        }
    }
}