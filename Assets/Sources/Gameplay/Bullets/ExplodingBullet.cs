using UnityEngine;

namespace Assets.Sources.Gameplay.Bullets
{
    public abstract class ExplodingBullet : Explosion
    {
        [SerializeField] private GameObject _projectile;

        private float _explosionRadius;
        private uint _explosionForce;
        private uint _damage;

        public Vector3 StartPosition;
        protected GameObject Projectile => _projectile;

        public virtual ExplodingBullet BindExplosionSettings(float explosionRadius, uint explosionForce, uint damage)
        {
            _explosionRadius = explosionRadius;
            _explosionForce = explosionForce;
            _damage = damage;

            StartPosition = transform.position;

            return this;
        }

        protected void Explode(Vector3 position) =>
            Explode(position, _explosionRadius, _explosionForce, _damage);
    }
}
