using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.Destructions.AllDestructionBuilding
{
    public class AllDestructionBuildingPart : DestructionPart, IDamageable, IDestructablePart
    {
        public event Action<Vector3, uint> Destructed;

        public Transform Transform => transform;

        public void TakeDamage(ExplosionInfo explosionInfo) =>
            Destructed?.Invoke(explosionInfo.ExplosionPosition, explosionInfo.ExplosionForce);
    }
}
