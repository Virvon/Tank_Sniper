using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Points
{
    public class EnemyCarPoint : PatrolingEnemyPoint
    {
        protected override Vector3 GetEnemySize() =>
            StartPoint.rotation * new Vector3(2, 2, 3);
    }
}