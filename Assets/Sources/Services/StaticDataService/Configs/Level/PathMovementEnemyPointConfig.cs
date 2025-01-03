using Assets.Sources.Gameplay.Enemies.Points;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;

namespace Assets.Sources.Services.StaticDataService.Configs.Level
{
    public abstract class PathMovementEnemyPointConfig : StaticEnemyPointConfig
    {
        public PathPointConfig[] Path;
        public float Speed;

        public PathMovementEnemyPointConfig(
            string id,
            Vector3 startPosition,
            Quaternion startRotation,
            EnemyType enemyType,
            EnemyPathPoint[] path)
            : base(id, startPosition, startRotation, enemyType)
        {
            Path = path.Select(value => new PathPointConfig(value.transform.position, value.RotationAngle, value.RotationDelta)).ToArray();
        }
    }
}