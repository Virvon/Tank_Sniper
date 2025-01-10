using UnityEngine;

namespace Assets.Sources.Gameplay.Destructions
{
    public class BossDestructionPart : CollidingDestructionPart
    {
        public bool IsDesturcted { get; private set; }

        public override void Destruct(Vector3 explosionPosition, uint explosionForce)
        {
            IsDesturcted = true;
            base.Destruct(explosionPosition, explosionForce);
        }
    }
}
