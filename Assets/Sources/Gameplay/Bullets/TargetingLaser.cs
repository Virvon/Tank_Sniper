using UnityEngine;
using UnityEngine.VFX;

namespace Assets.Sources.Gameplay.Bullets
{
    public class TargetingLaser : Laser
    {
        private const string PositionValue = "Position";

        [SerializeField] private VisualEffect _projectileVisualEffect;

        public TargetingLaser BindTarget(Vector3 targetPosition)
        {
            _projectileVisualEffect.SetVector3(PositionValue, targetPosition);

            CreateExplosionParticle(targetPosition, Quaternion.identity);
            Explode(targetPosition);

            return this;
        }
    }
}
