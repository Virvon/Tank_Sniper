using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies
{
    public class EnemyPoint : MonoBehaviour
    {
        public string Id;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + new Vector3(0, 1, 0), new Vector3(1, 2, 1));
            Gizmos.DrawLine(transform.position + new Vector3(0, 1, 0), transform.position + new Vector3(0, 1, 0) + (transform.forward * 1.5f));
        }
    }
}