using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Points
{
    public class MovementEnemyEngineryPoint : PatrolingEnemyPoint
    {
        protected override Vector3 GetEnemySize() =>
            StartPoint.rotation * new Vector3(2, 2, 3);
    }
}