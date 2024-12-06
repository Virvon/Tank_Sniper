using UnityEngine;

namespace Assets.Sources.Gameplay.Bullets
{
    public abstract class ExplodingBullet : MonoBehaviour
    {
        private readonly Collider[] _overlapColliders = new Collider[32];

        [SerializeField] private ParticleSystem _explosionParticlePrefab;
        [SerializeField] private GameObject _projectile;

        private float _explosionRadius;
        private uint _explosionForce;

        protected GameObject Projectile => _projectile;

        public ExplodingBullet BindExplosionSettings(float explosionRadius, uint explosionForce)
        {
            _explosionRadius = explosionRadius;
            _explosionForce = explosionForce;

            return this;
        }

        protected void CreateExplosionParticle(Vector3 position, Quaternion rotation)
        {
            ParticleSystem explosionParticle = Instantiate(_explosionParticlePrefab, position, rotation, transform);
            explosionParticle.Play();
        }

        protected void GiveDamage(Vector3 position)
        {
            int overlapCount = Physics.OverlapSphereNonAlloc(position, _explosionRadius, _overlapColliders);

            for (int i = 0; i < overlapCount; i++)
            {
                if (_overlapColliders[i].TryGetComponent(out IDamageable damageable))
                    damageable.TakeDamage(position, _explosionForce);
            }
        }
    }
}
