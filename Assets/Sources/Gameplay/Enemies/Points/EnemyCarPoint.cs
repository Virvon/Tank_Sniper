using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies.Points
{
    public class EnemyCarPoint : EnemyPoint
    {
        public EnemyPathPoint[] Path;
        public uint MaxRotationAngle;
        public float Speed;
    }
}