using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Points
{
    [Serializable]
    public class EnemyCarPathPoint
    {
        public Vector3 Position;
        public uint RotationAngle;

        public EnemyCarPathPoint(Vector3 position) =>
            Position = position;
    }
}