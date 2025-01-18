using UnityEngine;
using UnityEngine.VFX;

namespace Assets.Sources.Gameplay.Bullets
{
    public class TargetingLaser : Laser
    {
        private const string PositionValue = "Position";
        private const float Size = 0.5f;

        [SerializeField] private LaserLine _laserLine;

        private Vector3 _targetPosition;

        public TargetingLaser BindTarget(Vector3 targetPosition, Vector3 startPosition)
        {
            _laserLine.Initialize(startPosition, targetPosition, Size);
            _laserLine.SetActive(true);

            _targetPosition = targetPosition;

            CreateExplosionParticle(targetPosition, Quaternion.identity);
            

            return this;
        }

        public override ExplodingBullet BindExplosionSettings(float explosionRadius, uint explosionForce, uint damage)
        {
            ExplodingBullet explodingBullet = base.BindExplosionSettings(explosionRadius, explosionForce, damage);
            Explode(_targetPosition);

            return explodingBullet;
        }
    }
}
