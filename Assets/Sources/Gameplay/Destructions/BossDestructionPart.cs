using UnityEngine;

namespace Assets.Sources.Gameplay.Destructions
{
    public class BossDestructionPart : DestructionPart
    {
        public bool IsDesturcted { get; private set; }

        protected override bool IsIgnoreRotation => true;

        public override void Destruct(Vector3 explosionPosition, uint explosionForce)
        {
            IsDesturcted = true;
            base.Destruct(explosionPosition, explosionForce);
        }
    }
}
