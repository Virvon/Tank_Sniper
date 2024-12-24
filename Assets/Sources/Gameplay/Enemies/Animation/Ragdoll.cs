using Assets.Sources.Gameplay.Destructions;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Animation
{
    public class Ragdoll : DestructionPart
    {
        [SerializeField] private Collider[] _colliders;
        [SerializeField] private Rigidbody _destructionRigidbody;

        public void SetActive(bool isActive)
        {
            foreach (Collider collider in _colliders)
                collider.enabled = isActive;
        }

        public override void Destruct(Vector3 bulletPosition, uint explosionForce)
        {
            SetActive(true);

            base.Destruct(bulletPosition, explosionForce);
        }

        protected override Rigidbody GetDestructionRigidbody() =>
            _destructionRigidbody;
    }
}
