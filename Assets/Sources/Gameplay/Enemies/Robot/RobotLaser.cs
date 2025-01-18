using Assets.Sources.Gameplay.Bullets;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Robot
{
    public class RobotLaser : MonoBehaviour
    {
        private const float Size = 0.5f;
        private const float MaxDistance = 300;

        [SerializeField] private LaserLine[] _lasers;

        Vector3 x;
        Vector3 y;

        public void SetLaser(Vector3 startPosition, Vector3 endPosition)
        {
            x = startPosition;
            y = endPosition;

            foreach (LaserLine laser in _lasers)
            {
                laser.Initialize(laser.transform.position, endPosition, Size);
                laser.SetActive(true);
            }
        }

        private void OnDrawGizmos()
        {
            if(x != null)
            {
                Gizmos.DrawLine(x, y);
            }
        }
    }
}
