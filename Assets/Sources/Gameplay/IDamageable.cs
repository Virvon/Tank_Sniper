using UnityEngine;

namespace Assets.Sources.Gameplay
{
    public interface IDamageable
    {
        void TakeDamage(Vector3 bulletPosition, uint explosionForce);
    }
}
