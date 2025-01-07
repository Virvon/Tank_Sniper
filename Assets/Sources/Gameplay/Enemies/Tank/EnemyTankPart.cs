using Assets.Sources.Gameplay.Destructions;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Tank
{
    public class EnemyTankPart : DestructionPart
    {
        [SerializeField] private Collider _collider;
        [SerializeField, Range(0, 1)] private float _damageModifier;

        public float DamageModifier => _damageModifier;

        public float GetDistanceTo(Vector3 explosionPosition) =>
            Vector3.Distance(explosionPosition, _collider.ClosestPoint(explosionPosition));

        public override void Destruct(Vector3 explosionPosition, uint explosionForce)
        {
            _collider.isTrigger = false;
            base.Destruct(explosionPosition, explosionForce);
        }
    }
}
