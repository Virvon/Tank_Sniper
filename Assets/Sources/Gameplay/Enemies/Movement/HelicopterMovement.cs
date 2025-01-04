using Assets.Sources.Gameplay.Player;
using Assets.Sources.Services.StaticDataService.Configs.Level.EnemyPoints;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies.Movement
{
    public class HelicopterMovement : MonoBehaviour
    {
        private const float StartEngineForce = 30;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _waitingTime = 12;
        [SerializeField] private Transform _helicopter;
        [SerializeField] private float _movingForce = 2;
        [SerializeField] private float _tiltForce = 20;
        [SerializeField] private float _turnTiltForce = 30;
        [SerializeField] private float _effectiveHeight = 30;
        [SerializeField] private uint _rotationSpeed = 60;
        [SerializeField] private Enemy _enemy;

        private PlayerTankWrapper _playerTankWrapper;
        private Aiming _aiming;

        private PathPointConfig[] _path;
        private bool _isWaitedAttack;

        private float _engineForce;
        private Vector2 _horizontalMovement;
        private Vector2 _hotizontalTilt;

        private PathPointConfig _targetPoint;
        private int _currentPointIndex;

        private bool _isMoved;
        private bool _canLookToPlayer;
        private bool _needLookToPlayer;
        private bool _isPointReached;
        private bool _isPathLooped;
        private bool _isDestructed;

        [Inject]
        private void Construct(PlayerTankWrapper playerTankWrapper, Aiming aiming)
        {
            _playerTankWrapper = playerTankWrapper;
            _aiming = aiming;

            _engineForce = StartEngineForce;
            _canLookToPlayer = false;
            _needLookToPlayer = false;
            _isPointReached = true;
            _isDestructed = false;

            _aiming.Shooted += OnPlayerShooted;
            _enemy.Destructed += OnEnemyDestructed;
        }

        private void OnDestroy()
        {
            _aiming.Shooted -= OnPlayerShooted;
            _enemy.Destructed -= OnEnemyDestructed;
        }

        private void FixedUpdate()
        {
            if (_isDestructed)
                return;

            Lift();
            Move();
            Tilt();
            Rotate();
        }

        public void Initialize(PathPointConfig[] path, bool isWaitedAttack, bool isPathLooped)
        {
            _path = path;
            _isWaitedAttack = isWaitedAttack;
            _isPathLooped = isPathLooped;

            _isMoved = _isWaitedAttack == false;

            StartCoroutine(Mover());
        }

        private void Rotate()
        {
            if (_isPointReached && (_canLookToPlayer == false || _needLookToPlayer == false))
                return;

            Vector2 lookDiretion = _canLookToPlayer && _needLookToPlayer ? (new Vector2(_playerTankWrapper.transform.position.x, _playerTankWrapper.transform.position.z) - new Vector2(transform.position.x, transform.position.z)).normalized : _horizontalMovement;

            if (lookDiretion == Vector2.zero)
                return;

            Quaternion rotation = Quaternion.LookRotation(new Vector3(lookDiretion.x, 0, lookDiretion.y), transform.up);
            _helicopter.rotation = Quaternion.RotateTowards(_helicopter.rotation, rotation, _rotationSpeed * Time.deltaTime);
            _helicopter.localRotation = Quaternion.Euler(0, _helicopter.localRotation.eulerAngles.y, _helicopter.localRotation.eulerAngles.z);
        }

        private void Move()
        {
            if (_targetPoint == null)
                return;

            Vector2 position = new Vector2(transform.position.x, transform.position.z);
            _horizontalMovement = (new Vector2(_targetPoint.Position.x, _targetPoint.Position.z) - position).normalized;

            if(_isPointReached == false)
            {
                _rigidbody.AddRelativeForce(Vector3.forward * Mathf.Max(0f, _horizontalMovement.y * _movingForce * _rigidbody.mass));
                _rigidbody.AddRelativeForce(Vector3.right * Mathf.Max(0f, _horizontalMovement.x * _movingForce * _rigidbody.mass));
            }
        }

        private void Lift()
        {
            float upForce = 1 - Mathf.Clamp(_rigidbody.transform.position.y / _effectiveHeight, 0, 1);
            upForce = Mathf.Lerp(0f, _engineForce, upForce) * _rigidbody.mass;
            _rigidbody.AddRelativeForce(Vector3.up * upForce);
        }

        private void Tilt()
        {
            float targetTiltX = _isPointReached ? 0 : _horizontalMovement.x * _turnTiltForce;
            float targetTiltY = _isPointReached ? 0 : _horizontalMovement.y * _tiltForce;

            _hotizontalTilt.x = Mathf.Lerp(_hotizontalTilt.x, targetTiltX, Time.deltaTime);
            _hotizontalTilt.y = Mathf.Lerp(_hotizontalTilt.y, targetTiltY, Time.deltaTime);
            _rigidbody.transform.localRotation = Quaternion.Euler(_hotizontalTilt.y, _rigidbody.transform.localEulerAngles.y, -_hotizontalTilt.x);
        }

        private void OnPlayerShooted()
        {
            _isMoved = true;
            _needLookToPlayer = true;
        }

        private void OnEnemyDestructed()
        {
            _isDestructed = true;
            _isMoved = false;
        }

        private IEnumerator Mover()
        {
            yield return new WaitWhile(() => _isMoved == false);

            _currentPointIndex = 0;

            while (_isMoved)
            {
                _targetPoint = _path[_currentPointIndex];

                _isPointReached = false;

                yield return new WaitWhile(() => Vector3.Distance(transform.position, _targetPoint.Position) > _targetPoint.RotationDelta);

                _isPointReached = true;
                _canLookToPlayer = true;

                yield return new WaitForSeconds(_waitingTime);

                _currentPointIndex++;

                if (_currentPointIndex >= _path.Length && _isPathLooped == false)
                    _isMoved = false;
                else if(_currentPointIndex >= _path.Length)
                    _currentPointIndex = 0;

                yield return null;
            }
        }
    }
}
