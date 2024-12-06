using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.Weapons
{
    public class TransmittingLaser : MonoBehaviour
    {
        private readonly Collider[] _overlapColliders = new Collider[32];

        private const float MaxDistance = 200;

        [SerializeField] private ParticleSystem _explosionParticlePrefab;
        [SerializeField] private GameObject _projetile;

        private float _explosionLifeTime;
        private float _explosionRadius;
        private uint _explosionForce;
        private IGameplayFactory _gamePlayFactory;

        [Inject]
        private void Construct(IGameplayFactory gameplayFactory)
        {
            _gamePlayFactory = gameplayFactory;
        }

        public void Initialize(float explosionLifeTime, float explosionRadius, uint explosionForce, float projectileLifeTime, int targetsCount)
        {
            bool isHitedEnemy = false;
            bool isHited;

            _explosionLifeTime = explosionLifeTime;
            _explosionRadius = explosionRadius;
            _explosionForce = explosionForce;

            List<IDamageable> hitedTargets = new();

            isHited = Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, MaxDistance);

            if (isHited)
            {
                ParticleSystem explosionParticle = Instantiate(_explosionParticlePrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal, transform.forward), transform);
                explosionParticle.Play();

                if (hitInfo.transform.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(hitInfo.point, _explosionForce);
                    hitedTargets.Add(damageable);

                    isHitedEnemy = true;
                }

                int overlapCount = Physics.OverlapSphereNonAlloc(hitInfo.point, _explosionRadius, _overlapColliders);

                for (int i = 0; i < overlapCount; i++)
                {
                    if (_overlapColliders[i].TryGetComponent(out damageable) && hitedTargets.Contains(damageable) == false)
                    {
                        damageable.TakeDamage(hitInfo.point, _explosionForce);
                        hitedTargets.Add(damageable);
                    }
                }
            }

            Destroy(gameObject, explosionLifeTime);
            Destroy(_projetile, projectileLifeTime);

            if (isHited == false)
                return;

            Enemy[] enemies = FindObjectsOfType<Enemy>();

            enemies = enemies.OrderBy(enemy => Vector3.Distance(hitInfo.point, enemy.transform.position)).Take(targetsCount).ToArray();

            Vector3 fitsPoint = Vector3.zero;

            if (isHitedEnemy)
            {
                fitsPoint = enemies.First().transform.position;
                enemies = enemies.Skip(1).ToArray();
            }
            else if (isHited)
            {
                fitsPoint = hitInfo.point;
            }

            _gamePlayFactory.CreteLaser2(Types.BulletType.Laser2, fitsPoint, enemies[1].transform.position);

            for(int i = 1; i < enemies.Length - 1; i++)
                _gamePlayFactory.CreteLaser2(Types.BulletType.Laser2, enemies[i].transform.position, enemies[i + 1].transform.position);
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<TransmittingLaser>>
        {
        }
    }
}
