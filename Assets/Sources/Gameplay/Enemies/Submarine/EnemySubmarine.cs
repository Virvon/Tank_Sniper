using Assets.Sources.Gameplay.Destructions;
using Assets.Sources.Gameplay.Player.Aiming;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies.Submarine
{
    public class EnemySubmarine : Enemy, IDamageable, IHealthable
    {
        private const float PossitionDelta = 0.3f;
        private const float DestructionDelta = 3;
        private const float ExplosionDuration = 0.2f;

        [SerializeField] private float _targetPositionY;
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private BossDestructionPart[] _parts;
        [SerializeField] private uint _health;
        [SerializeField] private EnemyEngineryExplosion[] _explosions;
        [SerializeField] private Rigidbody _submarine;

        private IShootedAiming _aiming;

        private bool _isPlayerShooted;

        public event Action<uint, uint> Damaged;

        public bool IsSurfaced { get; private set; }
        public bool IsDestructed { get; private set; }

        public uint MaxHealth { get; private set; }

        [Inject]
        private void Construct(IShootedAiming aiming)
        {
            _aiming = aiming;

            _isPlayerShooted = false;
            IsSurfaced = false;
            IsDestructed = false;
            MaxHealth = _health;

            _aiming.Shooted += OnPlayerShooted;
        }

        private void OnDestroy() =>
            _aiming.Shooted -= OnPlayerShooted;

        public void TakeDamage(ExplosionInfo explosionInfo)
        {
            if (IsDestructed)
                return;

            foreach (BossDestructionPart part in _parts)
            {
                if (part == null)
                    continue;

                if (Vector3.Distance(part.transform.position, explosionInfo.ExplosionPosition) < DestructionDelta
                    && part.IsDesturcted == false)
                    part.Destruct(explosionInfo.ExplosionPosition, explosionInfo.ExplosionForce);
            }

            TakeDamage(explosionInfo.Damage);

            if (_health == 0)
            {
                IsDestructed = true;
                OnDestructed();

                foreach(BossDestructionPart part in _parts)
                {
                    if (part == null || part.IsDesturcted)
                        continue;

                    part.transform.parent = null;
                    part.Destruct(explosionInfo.ExplosionPosition, explosionInfo.ExplosionForce);
                }

                _submarine.transform.parent = null;
                _submarine.isKinematic = false;

                StartCoroutine(Explosioner());
            }
        }

        private void TakeDamage(uint damage)
        {
            if (damage > _health)
                damage = _health;

            _health -= damage;

            Damaged?.Invoke(_health, damage);
        }

        private void OnPlayerShooted()
        {
            if (_isPlayerShooted)
                return;

            _isPlayerShooted = true;

            StartCoroutine(Mover());
        }

        private IEnumerator Mover()
        {
            while (Mathf.Abs(transform.position.y - _targetPositionY) > PossitionDelta)
            {
                _rigidbody.AddForce(Vector3.up * _speed * Time.deltaTime, ForceMode.VelocityChange);

                yield return null;
            }

            IsSurfaced = true;
        }

        private IEnumerator Explosioner()
        {
            WaitForSeconds delay = new WaitForSeconds(ExplosionDuration);

            foreach(EnemyEngineryExplosion explosion in _explosions)
            {
                explosion.Explode();
                yield return delay;
            }
        }
    }
}
