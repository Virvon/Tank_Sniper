using UnityEngine;

namespace Assets.Sources.Gameplay.Bullets
{
    public abstract class ExplodingBullet : Explosion
    {
        [SerializeField] private GameObject _projectile;

        private float _explosionRadius;
        private uint _explosionForce;
        private uint _damage;

        protected GameObject Projectile => _projectile;

        public ExplodingBullet BindExplosionSettings(float explosionRadius, uint explosionForce, uint damage)
        {
            _explosionRadius = explosionRadius;
            _explosionForce = explosionForce;
            _damage = damage;

            return this;
        }

        protected void Explode(Vector3 position) =>
            Explode(position, _explosionRadius, _explosionForce, _damage);
    }
}
