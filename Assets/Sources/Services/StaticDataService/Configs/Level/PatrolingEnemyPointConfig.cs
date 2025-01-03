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
    public class PatrolingEnemyPointConfig : PathMovementEnemyPointConfig
    {
        public uint MaxRotationAngle;
        public float StoppingDuration;

        public PatrolingEnemyPointConfig(string id, Vector3 startPosition, Quaternion startRotation, EnemyType enemyType, EnemyPathPoint[] path)
            : base(id, startPosition, startRotation, enemyType, path)
        {
        }

        public override async UniTask<Enemy> Create(IGameplayFactory gameplayFactory)
        {
            Enemy enemy = await base.Create(gameplayFactory);

            EnemyPatroling enemyPatroling = enemy.gameObject.AddComponent<EnemyPatroling>();
            enemyPatroling.Initialize(Path, Speed, MaxRotationAngle);
            enemyPatroling.Initialize(StoppingDuration);

            return enemy;
        }
    }
}