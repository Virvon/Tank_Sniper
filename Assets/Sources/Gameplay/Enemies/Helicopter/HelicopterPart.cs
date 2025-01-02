using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Helicopter
{
    public class HelicopterPart : MonoBehaviour, IDamageable
    {
        public event Action<ExplosionInfo> Damaged;
        public event Action Collided;

        public void TakeDamage(ExplosionInfo explosionInfo) =>
            Damaged?.Invoke(explosionInfo);

        public void OnCollisionEnter(Collision collision) =>
            Collided?.Invoke();
    }
}
