using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies
{
    public class EnemyEngineryExplosion : Explosion
    {
        [SerializeField] private float _explosionRadius;
        [SerializeField] private uint _damage;
        [SerializeField] private uint _explosionForce;
        [SerializeField] private float _explosionDuration;

        public uint ExplosionForce => _explosionForce;

        public void Explode()
        {
            CreateExplosionParticle(transform.position, Quaternion.identity);
            Explode(transform.position, _explosionRadius, _explosionForce, _damage);
            Destroy(gameObject, _explosionDuration);
        }
    }
}
