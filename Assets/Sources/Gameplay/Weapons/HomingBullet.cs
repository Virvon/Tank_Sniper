using Assets.Sources.Gameplay.Enemies;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.Weapons
{
    public class HomingBullet : MonoBehaviour
    {
        private readonly Collider[] _overlapColliders = new Collider[32];

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private GameObject _projectile;
        [SerializeField] private ParticleSystem _explosionParticlePrefab;

        private float _searchRadius;
        private float _flyingSpeed;
        private float _explosionRadius;
        private uint _explosionForce;
        private float _rotationSpeed;
        private float _explosionLifeTime;
        private float _targetingDelay;
        private Enemy _target;
        private bool _isFollowed;
        private GameplayCamera _gameplayCamera;

        [Inject]
        private void Construct(GameplayCamera gameplayCamera)
        {
            _gameplayCamera = gameplayCamera;
        }

        private void OnCollisionEnter(Collision collision)
        {
            _isFollowed = false;

            _rigidbody.isKinematic = true;

            List<IDamageable> hitedTargets = new();

            ParticleSystem explosionParticle = Instantiate(_explosionParticlePrefab, transform.position, Quaternion.identity, transform);
            explosionParticle.Play();

            if (collision.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(transform.position, _explosionForce);
                hitedTargets.Add(damageable);
            }

            int overlapCount = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, _overlapColliders);

            for (int i = 0; i < overlapCount; i++)
            {
                if (_overlapColliders[i].TryGetComponent(out damageable) && hitedTargets.Contains(damageable) == false)
                {
                    damageable.TakeDamage(transform.position, _explosionForce);
                    hitedTargets.Add(damageable);
                }
            }

            Destroy(_projectile);
            Destroy(gameObject, _explosionLifeTime);
        }

        public void Initialize(float searchRadius, float flyingSpeed, float explosiionRadius, uint explosionForce, float rotationSpeed, float explosionLifeTime, float targetingDelay)
        {
            _searchRadius = searchRadius;
            _flyingSpeed = flyingSpeed;
            _explosionRadius = explosiionRadius;
            _explosionForce = explosionForce;
            _rotationSpeed = rotationSpeed;
            _explosionLifeTime = explosionLifeTime;
            _targetingDelay = targetingDelay;

            _isFollowed = false;
            _isFollowed = false;

            _rigidbody.velocity = transform.forward * _flyingSpeed;

            Enemy[] enemies = FindObjectsOfType<Enemy>();

            Vector3 center = _gameplayCamera.Camera.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, 0));          

            enemies = enemies.Where(enemy => Vector3.Distance(center, _gameplayCamera.Camera.WorldToScreenPoint(new Vector3(enemy.transform.position.x, enemy.transform.position.y, 1))) <= searchRadius).ToArray();

            if(enemies.Length > 0)
            {
                _target = enemies[Random.Range(0, enemies.Length)];

                StartCoroutine(Follower());
            }
        }

        private IEnumerator Follower()
        {
            _isFollowed = true;

            yield return new WaitForSeconds(_targetingDelay);

            while (_isFollowed)
            {
                Quaternion targetRotation = transform.rotation * Quaternion.FromToRotation(transform.forward, (_target.transform.position - transform.position).normalized);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
                _rigidbody.velocity = transform.forward * _flyingSpeed;

                yield return null;
            }
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<HomingBullet>>
        {
        }
    }
}
