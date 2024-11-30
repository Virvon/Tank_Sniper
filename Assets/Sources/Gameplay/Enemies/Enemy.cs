using Assets.Sources.Gameplay.Enemies.Animation;
using Assets.Sources.Services.StaticDataService.Configs.Level;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private Animator _animator;
        [SerializeField] private EnemyAnimation _enemyAnimation;
        [SerializeField] private float _rotationSpeed;

        public Animator Animator => _animator;
        public EnemyPointConfig Config { get; private set; }

        protected Transform ShootPoint => _shootPoint;


        public void TakeDamage(Vector3 attackPosition)
        {
            _rigidBody.isKinematic = false;
            _rigidBody.AddForce((transform.position - attackPosition).normalized * 1200, ForceMode.Impulse);

            Destroy(gameObject, 5);
        }

        public void StartShooting()
        {
            
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<Enemy>>
        {
        }
    }
}
