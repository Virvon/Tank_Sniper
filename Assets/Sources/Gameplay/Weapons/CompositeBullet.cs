using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.Weapons
{
    public class CompositeBullet : MonoBehaviour
    {
        private readonly Collider[] _overlapColliders = new Collider[32];

        [SerializeField] private ParticleSystem _explosionParticleSystemPrefab;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private Collider _collider;
        [SerializeField] private GameObject _projectile;

        private IGameplayFactory _gameplayFactory;
        private float _explosionLifeTime;
        private uint _explosionForce;
        private float _explosionRadius;
        private uint _bombsCount;
        private float _flySpeed;

        [Inject]
        private void Construct(IGameplayFactory gameplayFactory)
        {
            _gameplayFactory = gameplayFactory;
        }

        public void Initialize(float explosionLifetime, uint explosionForce, float explosionRadius, uint bombsCount, float flySpeed)
        {
            _explosionLifeTime = explosionLifetime;
            _explosionForce = explosionForce;
            _explosionRadius = explosionRadius;
            _bombsCount = bombsCount;
            _flySpeed = flySpeed;

            _rigidBody.velocity = transform.forward * _flySpeed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.isKinematic = true;
            _collider.enabled = false;

            ParticleSystem explosionParticle = Instantiate(_explosionParticleSystemPrefab, transform.position, Quaternion.identity, transform);
            explosionParticle.Play();

            int overlapCount = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, _overlapColliders);

            for (int i = 0; i < overlapCount; i++)
            {
                if (_overlapColliders[i].TryGetComponent(out IDamageable damageable))
                    damageable.TakeDamage(transform.position, _explosionForce);
            }

            Destroy(gameObject, _explosionLifeTime);
            Destroy(_projectile);

            for(int i = 0; i < _bombsCount; i++)
            {
                Vector3 bombDirection = Random.onUnitSphere;
                bombDirection.y = 2;
                bombDirection.Normalize();

                Debug.Log(bombDirection);

                _gameplayFactory.CreateBomb(Types.BulletType.Bomb, transform.position, Quaternion.LookRotation(bombDirection));
            }
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<CompositeBullet>>
        {
        }
    }
}
