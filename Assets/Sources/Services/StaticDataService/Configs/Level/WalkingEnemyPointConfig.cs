using System.Linq;
using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.Level
{
    [Serializable]
    public class WalkingEnemyPointConfig : EnemyPointConfig
    {
        public Vector3[] Points;

        public WalkingEnemyPointConfig(string id, EnemyType enemyType, Transform[] points)
            : base(id, points[0].position, points[0].rotation, enemyType) =>
            Points = points.Select(point => point.position).ToArray();

        public override async UniTask<Enemy> Create(IGameplayFactory gameplayFactory)
        {
            Enemy enemy = await base.Create(gameplayFactory);

            enemy.GetComponent<Walking>().Init(Points);

            return enemy;
        }
    }
}