using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class PlayerTank : MonoBehaviour, IDamageable
    {
        [SerializeField] private uint _maxHealth;

        public event Action Attacked;
        public event Action HealthChanged;

        public uint Health { get; private set; }
        public uint MaxHealth => _maxHealth;

        private void Start()
        {
            Health = _maxHealth;
        }

        public void Attack()
        {
            Attacked?.Invoke();
        }

        public void TakeDamage(Vector3 bulletPosition, uint explosionForce)
        {
            if (Health == 0)
                return;

            Health--;
            HealthChanged?.Invoke();
        }

        public class Factory : PlaceholderFactory<string, UniTask<PlayerTank>>
        {
        }
    }
}
