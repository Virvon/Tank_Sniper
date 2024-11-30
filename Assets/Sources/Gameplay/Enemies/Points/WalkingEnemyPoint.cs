using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Points
{
    public class WalkingEnemyPoint : EnemyPoint
    {
        public Transform[] Path;

        protected void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, 0.1f);

            if (Path == null || Path.Length == 0)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Path[0].position + new Vector3(0, 1, 0), new Vector3(1, 2, 1));
            Gizmos.DrawLine(Path[0].position + new Vector3(0, 1, 0), Path[0].position + new Vector3(0, 1, 0) + Path[0].forward * 1.5f);

            for(int i = 0; i < Path.Length; i++)
            {
                Transform nextPoint = i == Path.Length - 1 ? Path[0] : Path[i + 1];

                Gizmos.DrawLine(Path[i].position, nextPoint.position);
                Gizmos.DrawSphere(Path[i].position, 0.2f);
            }
        }
    }
}