using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Assets.Sources.Types;
using Assets.Sources.Gameplay.Enemies.Points;
using Assets.Sources.Gameplay.Enemies.Movement;

namespace Assets.Sources.Services.StaticDataService.Configs.Level.EnemyPoints
{
    [Serializable]
    public class HelicopterPointConfig : PathMovementEnemyPointConfig
    {
        public bool IsWaitedAttack;
        public bool IsPathLooped;

        public HelicopterPointConfig(string id, Vector3 startPosition, Quaternion startRotation, EnemyType enemyType, EnemyPathPoint[] path)
            : base(id, startPosition, startRotation, enemyType, path)
        {
        }

        public override async UniTask<Enemy> Create(IGameplayFactory gameplayFactory)
        {
            Enemy enemy = await gameplayFactory.CreateEnemy(EnemyType, StartPosition, StartRotation);

            enemy.GetComponent<HelicopterMovement>().Initialize(Path, IsWaitedAttack, IsPathLooped);

            return enemy;
        }
    }
}