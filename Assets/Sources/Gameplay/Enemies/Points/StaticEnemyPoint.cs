using Assets.Sources.Types;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Points
{
    public class StaticEnemyPoint : MonoBehaviour
    {
        public string Id;
        public EnemyType EnemyType;
        public Transform StartPoint;

        protected virtual void OnDrawGizmos()
        {
            if (StartPoint == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(StartPoint.position + new Vector3(0, 1, 0), GetEnemySize());
            Gizmos.DrawLine(StartPoint.position + new Vector3(0, 1, 0), StartPoint.position + new Vector3(0, 1, 0) + StartPoint.forward * 1.5f);
        }

        protected virtual Vector3 GetEnemySize() =>
            new Vector3(1, 2, 1);
    }
}