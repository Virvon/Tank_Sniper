using UnityEngine;

namespace Assets.Sources.Gameplay.Bullets
{
    public class DirectionalLaser : Laser
    {
        private const float MaxDistance = 200;

        public override Laser BindLifeTimes(float explosionLifeTime, float projectileLifeTime)
        {
            Launch(out RaycastHit _);

            return base.BindLifeTimes(explosionLifeTime, projectileLifeTime);
        }

        protected bool Launch(out RaycastHit hitInfo)
        {
            bool isHited = Physics.Raycast(transform.position, transform.forward, out hitInfo, MaxDistance);

            if (isHited)
            {
                CreateExplosionParticle(hitInfo.point, Quaternion.LookRotation(hitInfo.normal, transform.forward));
                Explode(hitInfo.point);
            }

            return isHited;
        }
    }
}
