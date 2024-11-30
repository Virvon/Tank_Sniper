using Assets.Sources.Services.StaticDataService.Configs.Level;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;

        protected PathPointConfig[] Path;
        protected uint MaxRotationAngle;
        protected float Speed;

        private bool _isMoved;
        private uint _currentPointIndex;
        private Coroutine _mover;

        protected event Action PointFinished;
        protected event Action NextPointStarted;

        protected virtual float StoppingDuration => 0;

        private void Start() =>
            _currentPointIndex = 0;

        protected void StartMovement()
        {
            if (_mover != null)
                StopCoroutine(_mover);

            _mover = StartCoroutine(Mover());
        }

        protected void StopMovement()
        {
            _isMoved = false;
            StopCoroutine(_mover);
        }

        private IEnumerator Mover()
        {
            _isMoved = true;

            WaitForSeconds stoppingDuration = new WaitForSeconds(StoppingDuration);

            while (_isMoved)
            {
                PathPointConfig targetPoint = _currentPointIndex < Path.Length - 1 ? Path[_currentPointIndex + 1] : Path[0];
                float rotationAngle = Path[_currentPointIndex].RotationAngle == 0 ? MaxRotationAngle : Mathf.Clamp(Path[_currentPointIndex].RotationAngle, 0, MaxRotationAngle);

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

                if (_currentPointIndex == Path.Length - 1 && CanMoveNextCircle() == false)
                    _isMoved = false;
                else if(_currentPointIndex > Path.Length - 1)
                    _currentPointIndex = 0;

                PointFinished?.Invoke();

                yield return null;
            }
        }

        protected virtual bool CanMoveNextCircle() =>
            true;
    }
}
