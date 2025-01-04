using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Gameplay.Enemies.Movement;
using Assets.Sources.Gameplay.Enemies.Points;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.Level.EnemyPoints
{
    [Serializable]
    public class EnemyMovementEngineryPointConfig : HelicopterPointConfig
    {
        public uint MaxRotationAngle;
        public float SpeedAfterAttack;

        public EnemyMovementEngineryPointConfig(string id, Vector3 startPosition, Quaternion startRotation, EnemyType enemyType, EnemyPathPoint[] path)
            : base(id, startPosition, startRotation, enemyType, path)
        {
        }

        public override async UniTask<Enemy> Create(IGameplayFactory gameplayFactory)
        {
            Enemy enemy = await gameplayFactory.CreateEnemy(EnemyType, StartPosition, StartRotation);

            EnemyEngineryMovement enemyEngineryMovement = enemy.gameObject.AddComponent<EnemyEngineryMovement>();
            enemyEngineryMovement.Initialize(Path, Speed, MaxRotationAngle);
            enemyEngineryMovement.Initialize(SpeedAfterAttack);

            return enemy;
        }
    }
}