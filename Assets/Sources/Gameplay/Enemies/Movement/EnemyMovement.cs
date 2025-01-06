using Assets.Sources.Services.StaticDataService.Configs.Level.EnemyPoints;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Movement
{
    public class EnemyMovement : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private Enemy _enemy;

        private PathPointConfig[] _path;
        private uint _maxRotationAngle;
        private float _speed;

        private bool _isMoved;
        private uint _currentPointIndex;
        private Coroutine _mover;

        protected virtual float Speed => _speed;

        public event Action PointFinished;
        public event Action NextPointStarted;

        protected virtual float StoppingDuration => 0;

        private void OnDestroy() =>
            _enemy.Destructed -= StopMovement;

        public void Initialize(PathPointConfig[] path, float speed, uint maxRotationAngle)
        {
            _path = path;
            _speed = speed;
            _maxRotationAngle = maxRotationAngle;

            _rigidbody = GetComponent<Rigidbody>();
            _enemy = GetComponent<Enemy>();

            _currentPointIndex = 0;

            _enemy.Destructed += StopMovement;
        }

        protected void StartMovement()
        {
            StopMovement();

            _mover = StartCoroutine(Mover());
        }

        protected void StopMovement()
        {
            _isMoved = false;
            
            if(_mover != null)
                StopCoroutine(_mover);
        }

        private IEnumerator Mover()
        {
            _isMoved = true;

            WaitForSeconds stoppingDuration = new WaitForSeconds(StoppingDuration);

            while (_isMoved)
            {
                PathPointConfig targetPoint = _currentPointIndex < _path.Length - 1 ? _path[_currentPointIndex + 1] : _path[0];
                float rotationAngle = _path[_currentPointIndex].RotationAngle == 0 ? _maxRotationAngle : Mathf.Clamp(_path[_currentPointIndex].RotationAngle, 0, _maxRotationAngle);

                NextPointStarted?.Invoke();

                while (Vector3.Distance(transform.position, targetPoint.Position) > targetPoint.RotationDelta)
                {
                    Vector3 currentPosition = new Vector3(transform.position.x, targetPoint.Position.y, transform.position.z);
                    Quaternion targetRotation = Quaternion.LookRotation((targetPoint.Position - currentPosition).normalized);

                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationAngle * Time.deltaTime);

                    _rigidbody.velocity = transform.forward * Speed;

                    yield return null;
                }

                _currentPointIndex++;

                if (_currentPointIndex == _path.Length - 1 && CanMoveNextCircle() == false)
                    _isMoved = false;
                else if (_currentPointIndex > _path.Length - 1)
                    _currentPointIndex = 0;

                PointFinished?.Invoke();

                yield return stoppingDuration;
            }
        }

        protected virtual bool CanMoveNextCircle() =>
            true;
    }
}
