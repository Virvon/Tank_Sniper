using Assets.Sources.Gameplay.Destructions;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Animation
{
    public class Ragdoll : DestructionPart
    {
        [SerializeField] private Collider[] _colliders;
        [SerializeField] private Rigidbody[] _rigidbodies;
        [SerializeField] private Rigidbody _destructionRigidbody;

        private void Start() =>
            SetActive(false);

        public override void Destruct(Vector3 bulletPosition, uint explosionForce)
        {
            SetActive(true);

            base.Destruct(bulletPosition, explosionForce);
        }

        protected override Rigidbody GetDestructionRigidbody() =>
            _destructionRigidbody;

        private void SetActive(bool isActive)
        {
            foreach (Collider collider in _colliders)
                collider.enabled = isActive;

            foreach (Rigidbody rigidbody in _rigidbodies)
                rigidbody.isKinematic = isActive == false;
        }
    }
}
