using Assets.Sources.Gameplay.Destructions;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Robot
{
    public class ArmorPart : MonoBehaviour
    {
        [SerializeField] private DestructionPart[] _destructionParts;

        public bool IsDestructed { get; private set; }

        private void Start() =>
            IsDestructed = false;

        public void Destruct(Vector3 explosionPosition, uint explosionForce)
        {
            IsDestructed = true;

            foreach (DestructionPart destructionPart in _destructionParts)
            {
                destructionPart.transform.parent = null;
                destructionPart.Destruct(explosionPosition, explosionForce);
            }
        }
    }
}
