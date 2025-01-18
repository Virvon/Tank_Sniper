using UnityEngine;
using UnityEngine.VFX;

namespace Assets.Sources.Gameplay.Bullets
{
    public class TargetingLaser : Laser
    {
        private const string PositionValue = "Position";
        private const float Size = 0.5f;

        [SerializeField] private LaserLine _laserLine;

        public TargetingLaser BindTarget(Vector3 targetPosition, Vector3 startPosition)
        {
            _laserLine.Initialize(startPosition, targetPosition, Size);
            _laserLine.SetActive(true);

            CreateExplosionParticle(targetPosition, Quaternion.identity);
            Explode(targetPosition);

            return this;
        }
    }
}
