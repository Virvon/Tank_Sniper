using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using System.Linq;

namespace Assets.Sources.Services.StaticDataService.Configs.Level
{
    [Serializable]
    public class HelicopterPointConfig : EnemyPointConfig
    {
        public PathPointConfig[] Path;
        public uint MaxRotationAngle;
        public float Speed;
        public bool IsWaitedAttack;

        public HelicopterPointConfig(
            string id,
            EnemyType enemyType,
            EnemyPathPoint[] path,
            Transform startPoint,
            uint maxRotationAngle,
            float speed)
            : base(id, startPoint.position, startPoint.rotation, enemyType)
        {
            Path = path.Select(value => new PathPointConfig(value.transform.position, value.RotationAngle, value.RotationDelta)).ToArray();
            MaxRotationAngle = maxRotationAngle;
            Speed = speed;
        }

        public override async UniTask<Enemy> Create(IGameplayFactory gameplayFactory)
        {
            Enemy enemy = await base.Create(gameplayFactory);

            enemy.GetComponent<HelicopterMovement>().Initialize(this);

            return enemy;
        }
    }
}