using UnityEngine;

namespace Assets.Sources.Gameplay.Destructions.AllDestructionBuilding
{
    public class AllDestructinBuilding : MonoBehaviour
    {
        private AllDestructionBuildingPart[] _parts;

        private bool _isBreaked;

        private void Start()
        {
            _parts = GetComponentsInChildren<AllDestructionBuildingPart>();
            _isBreaked = false;

            foreach (AllDestructionBuildingPart part in _parts)
                part.Damaged += OnPartDamaged;
        }

        private void OnDestroy()
        {
            foreach (AllDestructionBuildingPart part in _parts)
                part.Damaged -= OnPartDamaged;
        }

        private void OnPartDamaged(Vector3 bulletPosition, uint explosionForce)
        {
            if (_isBreaked)
                return;

            _isBreaked = true;

            foreach (AllDestructionBuildingPart part in _parts)
            {
                part.Destruct(bulletPosition, explosionForce);
                part.OnDestruct(bulletPosition, explosionForce);
            }
        }
    }
}
