using Assets.Sources.Gameplay.Enemies.Animation;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
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
        [SerializeField] private Weapon _weapon;

        public Animator Animator => _animator;

        public void TakeDamage(Vector3 attackPosition)
        {
            _rigidBody.isKinematic = false;
            _rigidBody.AddForce((transform.position - attackPosition).normalized * 1200, ForceMode.Impulse);

            Destroy(gameObject, 5);
        }

        public void StartShooting() =>
            _weapon.StartShooting();

        public void Rotate(PlayerTank playerTank, Action callback)
        {
            Vector3 targetDirection = (playerTank.transform.position - transform.position).normalized;
            Vector3 shootPointForward = _shootPoint.forward;

            targetDirection.y = 0;
            shootPointForward.y = 0;

            Quaternion targetRotation = Quaternion.Euler(
                0,
                transform.rotation.eulerAngles.y + Quaternion.FromToRotation(shootPointForward, targetDirection).eulerAngles.y,
                0);

            StartCoroutine(Rotater(targetRotation, callback));
        }

        private IEnumerator Rotater(Quaternion targetRotation, Action callback)
        {
            Quaternion startRotation = transform.rotation;

            float rotationSpeed = 4;
            float progress = 0;

            while(transform.rotation != targetRotation)
            {
                progress += Time.deltaTime * rotationSpeed;

                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, progress);

                yield return null;
            }

            callback?.Invoke();
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<Enemy>>
        {
        }
    }
}
