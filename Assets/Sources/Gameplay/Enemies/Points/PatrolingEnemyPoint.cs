using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Points
{
    public class PatrolingEnemyPoint : StaticEnemyPoint
    {
        public EnemyPathPoint[] Path;

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.DrawSphere(transform.position, 0.1f);

            if (Path == null || Path.Length == 0)
                return;

            Gizmos.color = Color.red;

            for(int i = 0; i < Path.Length; i++)
            {
                Transform nextPoint = i == Path.Length - 1 ? Path[0].transform : Path[i + 1].transform;

                Gizmos.DrawLine(Path[i].transform.position, nextPoint.position);
                Gizmos.DrawSphere(Path[i].transform.position, 0.2f);
            }
        }
    }
}