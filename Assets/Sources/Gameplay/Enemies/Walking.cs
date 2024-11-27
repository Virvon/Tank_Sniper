using Assets.Sources.Gameplay.Enemies.Animation;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies
{
    public class Walking : MonoBehaviour
    {
        private const float Delta = 0.4f;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _stoppingDuration;

        private Vector3[] _points;

        private bool _isWalked;
        private uint _currentPointIndex;
        private Coroutine _walker;

        private void Start()
        {
            _walker = StartCoroutine(Walker());
        }

        public void Init(Vector3[] points)
        {
            _points = points;
        }

        public void StartWalking()
        {
            _isWalked = true;
        }

        public void StopWalking()
        {
            _isWalked = false;
            StopCoroutine(_walker);
        }

        private IEnumerator Walker()
        {
            WaitForSeconds stoppingDuration = new WaitForSeconds(_stoppingDuration);

            while (_isWalked)
            {
                Vector3 targetPoint = _currentPointIndex < _points.Length - 1 ? _points[_currentPointIndex + 1] : _points[0];

                _animator.SetBool(AnimationPath.IsWalked, true);

                while (Vector3.Distance(transform.position, targetPoint) > Delta)
                {
                    Vector3 currentPosition = new Vector3(transform.position.x, targetPoint.y, transform.position.z);
                    Quaternion targetRotation = Quaternion.LookRotation((targetPoint - currentPosition).normalized);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

                    _rigidbody.velocity = transform.forward;

                    yield return null;
                }

                _currentPointIndex = _currentPointIndex >= _points.Length ? 0 : _currentPointIndex + 1;

                _animator.SetBool(AnimationPath.IsWalked, false);

                yield return stoppingDuration;
            }
        }
    }
}
