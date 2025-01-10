using Assets.Sources.Gameplay.Destructions;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Helicopter.BossHelicopter
{
    public class BossHelicopterWeaponPart : CollidingDestructionPart
    {
        [SerializeField] private BossHelicopterShooting _shooting;

        public bool IsDesturcted { get; private set; }

        public override void Destruct(Vector3 explosionPosition, uint explosionForce)
        {
            IsDesturcted = true;
            _shooting.Destruct();
            base.Destruct(explosionPosition, explosionForce);
        }
    }
}
