using Assets.Sources.Gameplay.Enemies.Points;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies
{
    public class Riding : MonoBehaviour
    {
        private const float Delta = 5;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed;
        [SerializeField] private float _maxRotationAnge;

        private EnemyCarPathPoint[] _points;

        private bool _isRiding;
        private uint _currentPointIndex;
        private Coroutine _rider;

        private void Start()
        {
            StartRiding();
            _rider = StartCoroutine(Rider());
        }

        public void Init(EnemyCarPathPoint[] points)
        {
            _points = points;
        }

        public void StartRiding()
        {
            _isRiding = true;
        }

        public void StopRiding()
        {
            _isRiding = false;
            StopCoroutine(_rider);
        }

        private IEnumerator Rider()
        {
            while (_isRiding)
            {
                EnemyCarPathPoint targetPoint = _currentPointIndex < _points.Length - 1 ? _points[_currentPointIndex + 1] : _points[0];
                float rotationAngle = Mathf.Clamp(_points[_currentPointIndex].RotationAngle, 0, _maxRotationAnge);

                while (Vector3.Distance(transform.position, targetPoint.Position) > Delta)
                {
                    Vector3 currentPosition = new Vector3(transform.position.x, targetPoint.Position.y, transform.position.z);
                    Quaternion targetRotation = Quaternion.LookRotation((targetPoint.Position - currentPosition).normalized);

                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationAngle * Time.deltaTime);

                    _rigidbody.velocity = transform.forward * _speed;

                    yield return null;
                }

                _currentPointIndex = _currentPointIndex >= _points.Length - 1 ? 0 : _currentPointIndex + 1;

                yield return null;
            }
        }
    }
}
