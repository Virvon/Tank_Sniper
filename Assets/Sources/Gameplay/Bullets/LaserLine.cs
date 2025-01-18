using UnityEngine;

namespace Assets.Sources.Gameplay.Bullets
{
    public class LaserLine : MonoBehaviour
    {
        [SerializeField] private LineRenderer _laserRenderer;

        private Vector3 _startPosition;
        private Vector3 _endPosition;

        private bool _isActive;

        public void Initialize(Vector3 startPosition, Vector3 endPostion, float size)
        {
            _laserRenderer.startWidth = size;
            _laserRenderer.endWidth = size;

            _startPosition = startPosition;
            _endPosition = endPostion;
        }

        public void SetActive(bool isActive) =>
            _isActive = isActive;

        private void Update()
        {
            if (_startPosition == Vector3.zero || _endPosition == Vector3.zero || _isActive == false)
                return;

            _laserRenderer.SetPosition(0, _startPosition);
            _laserRenderer.SetPosition(1, _endPosition);
        }
    }
}
