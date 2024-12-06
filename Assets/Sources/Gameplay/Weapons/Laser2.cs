using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.VFX;
using Zenject;

namespace Assets.Sources.Gameplay.Weapons
{
    public class Laser2 : MonoBehaviour
    {
        private const string PositionValue = "Position";

        private readonly Collider[] _overlapColliders = new Collider[32];

        [SerializeField] private VisualEffect _projectileVisualEffect;
        [SerializeField] private ParticleSystem _explosionParticlePrefab;

        public void Initialize(Vector3 targetPosition, float projectileLifeTime, float explosionLifeTime, float explosionRadius, uint explosionForce)
        {
            _projectileVisualEffect.SetVector3(PositionValue, targetPosition);

            ParticleSystem explosionParticle = Instantiate(_explosionParticlePrefab, targetPosition, Quaternion.identity, transform);
            explosionParticle.Play();

            Destroy(_projectileVisualEffect.gameObject, projectileLifeTime);
            Destroy(gameObject, explosionLifeTime);

            int overlapCount = Physics.OverlapSphereNonAlloc(targetPosition, explosionRadius, _overlapColliders);

            for (int i = 0; i < overlapCount; i++)
            {
                if (_overlapColliders[i].TryGetComponent(out IDamageable damageable))
                    damageable.TakeDamage(targetPosition, explosionForce);
            }
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, UniTask<Laser2>>
        {
        }
    }
}
