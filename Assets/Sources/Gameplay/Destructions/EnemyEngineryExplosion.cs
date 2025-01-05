using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Destructions
{
    public class EnemyEngineryExplosion : Explosion
    {
        [SerializeField] private float _explosionDuration;

        private float _explosionRadius;
        private uint _damage;
        private uint _explosionForce;

        public uint ExplosionForce => _explosionForce;

        [Inject]
        private void Construct(IStaticDataService staticDataService)
        {
            EnviromentExplosionsConfig enviromentExplosionsConfig = staticDataService.EnviromentExplosionsConfig;

            _explosionRadius = enviromentExplosionsConfig.ExplosionRadius;
            _damage = enviromentExplosionsConfig.Damage;
            _explosionForce = enviromentExplosionsConfig.ExplosionForce;
        }

        public void Explode()
        {
            CreateExplosionParticle(transform.position, Quaternion.identity);
            Explode(transform.position, _explosionRadius, _explosionForce, _damage);
            Destroy(gameObject, _explosionDuration);
        }
    }
}
