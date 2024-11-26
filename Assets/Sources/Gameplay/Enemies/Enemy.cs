using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private Transform _shootPoint;

        private IGameplayFactory _gameplayFactory;

        public Transform ShootPoint => _shootPoint;

        [Inject]
        private void Construct(IGameplayFactory gameplayFactory)
        {
            _gameplayFactory = gameplayFactory;
        }

        public void TakeDamage(Vector3 attackPosition)
        {
            _rigidBody.isKinematic = false;
            _rigidBody.AddForce((transform.position - attackPosition).normalized * 1200, ForceMode.Impulse);

            Destroy(gameObject, 5);
        }

        public void Shoot(Vector3 direciton)
        {
            _gameplayFactory.CreateBullet(_shootPoint.position, Quaternion.LookRotation(direciton));
        }

        public class Factory : PlaceholderFactory<string, Vector3, Quaternion, UniTask<Enemy>>
        {
        }
    }
}
