using UnityEngine;

namespace Assets.Sources.Gameplay.Destructions
{
    [RequireComponent(typeof(Collider))]
    public class CollidingDestructionPart : DestructionPart
    {
        private Collider _collider;

        private void Start()
        {
            _collider = GetComponent<Collider>();
            _collider.enabled = false;
        }

        public override void Destruct(Vector3 explosionPosition, uint explosionForce)
        {
            _collider.enabled = true;
            base.Destruct(explosionPosition, explosionForce);
        }
    }
}