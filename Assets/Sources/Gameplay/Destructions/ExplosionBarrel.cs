using UnityEngine;

namespace Assets.Sources.Gameplay.Destructions
{
    public class ExplosionBarrel : EnemyEngineryExplosion, IDamageable
    {
        [SerializeField] private GameObject _barrel;

        private bool _isExplosded;

        private void Start() =>
            _isExplosded = false;

        public void TakeDamage(ExplosionInfo explosionInfo)
        {
            if (_isExplosded)
                return;

            _isExplosded = true;

            Destroy(_barrel);
            Explode();
        }
    }
}
