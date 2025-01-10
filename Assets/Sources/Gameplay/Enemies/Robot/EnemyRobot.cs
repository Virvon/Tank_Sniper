using Assets.Sources.Gameplay.Enemies.Animation;
using Assets.Sources.Gameplay.Enemies.Movement;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Robot
{
    public class EnemyRobot : Enemy, IDamageable, IHealthable
    {
        private const float Delta = 3;
        private const float AngleDelta = 5;

        [SerializeField] private uint _health;
        [SerializeField] private Animator _animator;
        [SerializeField] private ArmorPart[] _armorParts;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private Ragdoll _ragdoll;

        private EnemyEngineryMovement _enemyEngineryMovement;

        private bool _isDestructed;
        private bool _isEnemyShooted;

        private Coroutine _rotater;

        public event Action<uint, uint> Damaged;

        public uint MaxHealth { get; private set; }
        public bool IsStopped { get; private set; }

        private void Start()
        {
            _enemyEngineryMovement = GetComponent<EnemyEngineryMovement>();
            MaxHealth = _health;

            _isEnemyShooted = false;
            IsStopped = _enemyEngineryMovement.IsWaitedAttack;
            _isDestructed = false;

            if (_enemyEngineryMovement.IsWaitedAttack == false)
                OnNexptPointStarted();

            _enemyEngineryMovement.NextPointStarted += OnNexptPointStarted;
            _enemyEngineryMovement.PointFinished += OnPointFinished;
            Aiming.Shooted += OnPlayerShooted;
        }

        private void OnDestroy()
        {
            _enemyEngineryMovement.NextPointStarted -= OnNexptPointStarted;
            _enemyEngineryMovement.PointFinished -= OnPointFinished;
            Aiming.Shooted -= OnPlayerShooted;
        }

        public void TakeDamage(ExplosionInfo explosionInfo)
        {
            if (_isDestructed)
                return;

            foreach (ArmorPart armorPart in _armorParts)
            {
                if (Vector3.Distance(armorPart.transform.position, explosionInfo.ExplosionPosition) <= Delta
                    && armorPart.IsDestructed == false)
                    armorPart.Destruct(explosionInfo.ExplosionPosition, explosionInfo.ExplosionForce);
            }

            TakeDamage(explosionInfo.Damage);

            if (_health == 0)
            {
                _isDestructed = true;
                OnDestructed();
                _animator.enabled = false;
                _ragdoll.transform.parent = null;
                _ragdoll.Destruct(transform.position, 0);
                Destroy(gameObject);
            }
        }

        private void TakeDamage(uint damage)
        {
            if (damage > _health)
                damage = _health;

            _health -= damage;

            Damaged?.Invoke(_health, damage);
        }

        private void OnPointFinished()
        {
            IsStopped = true;
            _animator.SetBool(AnimationPath.IsWalked, false);

            if (_isEnemyShooted)
            {
                if (_rotater != null)
                    StopCoroutine(_rotater);

                _rotater = StartCoroutine(Rotater());
            }
        }

        private void OnNexptPointStarted()
        {
            IsStopped = false;
            _animator.SetBool(AnimationPath.IsWalked, true);
        }

        private void OnPlayerShooted() =>
            _isEnemyShooted = true;

        private IEnumerator Rotater()
        {
            while (IsStopped)
            {
                Vector3 shootPointForward = _shootPoint.forward;
                Vector3 targetDirection = (PlayerWrapper.transform.position - _shootPoint.position).normalized;

                Quaternion targetRotation = Quaternion.Euler(
                0,
                transform.rotation.eulerAngles.y + Quaternion.FromToRotation(shootPointForward, targetDirection).eulerAngles.y,
                0);

                shootPointForward.y = 0;
                targetDirection.y = 0;

                if (Vector3.Angle(shootPointForward, targetDirection) > AngleDelta)
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

                yield return null;
            }
        }
    }
}
