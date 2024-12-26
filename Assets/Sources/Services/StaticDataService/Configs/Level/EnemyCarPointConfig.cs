using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Gameplay.Enemies.Movement;
using Assets.Sources.Gameplay.Enemies.Points;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.Level
{
    [Serializable]
    public class EnemyCarPointConfig : PatrolingEnemyPointConfig
    {
        public bool IsLooped;
        public bool IsWaitedAttack;
        public float SpeedAfterAttack;

        public EnemyCarPointConfig(string id, Vector3 startPosition, Quaternion startRotation, EnemyType enemyType, EnemyPathPoint[] path)
            : base(id, startPosition, startRotation, enemyType, path)
        {
        }

        public override async UniTask<Enemy> Create(IGameplayFactory gameplayFactory)
        {
            Enemy enemy = await base.Create(gameplayFactory);

            enemy.GetComponent<EnemyEngineryMovement>().Initialize(SpeedAfterAttack);

            return enemy;
        }
    }
}