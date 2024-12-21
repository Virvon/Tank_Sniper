using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.Destructions
{
    public class TreePart : DestructionPart, IDamageable
    {
        public event Action<Vector3, uint> Damaged;

        public void TakeDamage(Vector3 bulletPosition, uint explosionForce) =>
            Damaged?.Invoke(bulletPosition, explosionForce);
    }
}
