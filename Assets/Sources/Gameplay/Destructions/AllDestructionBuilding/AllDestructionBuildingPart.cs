using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.Destructions.AllDestructionBuilding
{
    public class AllDestructionBuildingPart : DestructionPart, IDamageable, IDestructablePart
    {
        public event Action<Vector3, uint> Destructed;
        public event Action<Vector3, uint> Damaged;

        public Transform Transform => transform;

        public void TakeDamage(ExplosionInfo explosionInfo) =>
            Damaged?.Invoke(explosionInfo.ExplosionPosition, explosionInfo.ExplosionForce);

        public void OnDestruct(Vector3 explosionPosition, uint explosionForce) =>
            Destructed?.Invoke(explosionPosition, explosionForce);

    }
}
