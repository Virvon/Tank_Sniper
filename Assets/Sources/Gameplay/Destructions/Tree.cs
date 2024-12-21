using UnityEngine;

namespace Assets.Sources.Gameplay.Destructions
{
    public class Tree : MonoBehaviour
    {
        private TreePart[] _treeParts;

        private bool _isBreaked;

        private void Start()
        {
            _treeParts = GetComponentsInChildren<TreePart>();
            _isBreaked = false;

            foreach (TreePart part in _treeParts)
                part.Damaged += OnPartDamaged;
        }

        private void OnDestroy()
        {
            foreach (TreePart part in _treeParts)
                part.Damaged -= OnPartDamaged;
        }

        private void OnPartDamaged(Vector3 bulletPosition, uint explosionForce)
        {
            if (_isBreaked)
                return;

            _isBreaked = true;

            foreach(TreePart part in _treeParts)
                part.Destruct(bulletPosition, explosionForce);
        }
    }
}
