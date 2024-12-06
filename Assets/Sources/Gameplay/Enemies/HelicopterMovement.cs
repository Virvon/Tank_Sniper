using Assets.Sources.Services.StaticDataService.Configs.Level;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies
{
    public class HelicopterMovement : MonoBehaviour
    {
        [SerializeField] private float _heightSpeed;
        [SerializeField] private bool _lookToTarget;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _filter2 = 15f;
        [SerializeField] private ForceMode _forceMode;
        [SerializeField] private float _waitingTime = 12;

        public float TurnForce = 3f;
        public float ForwardForce = 10f;
        public float ForwardTiltForce = 20f;
        public float TurnTiltForce = 30f;
        public float EffectiveHeight = 100f;

        public float turnTiltForcePercent = 1.5f;
        public float turnForcePercent = 1.3f;

        private float _engineForce;

        private Vector2 _horizontalMovement;
        private Vector2 _hotizontalTilt;
        private float _horizontalTurnAngle;

        Vector2 _forward;
        Vector2 _rightWar;

        private PathPointConfig[] _path;
        private PlayerTank _playerTank;

        private PathPointConfig _targetPoint;
        private int _currentPointIndex;

        [Inject]
        private void Construct(PlayerTank playerTank)
        {
            _playerTank = playerTank;
        }

        private void Start()
        {
            _engineForce = 30;

            StartCoroutine(Mover());
        }

        void FixedUpdate()
        {
            LiftProcess();
            MoveProcess();
            TiltProcess();
        }

        private void Update()
        {
            Vector2 input = Vector2.zero;

            if (_horizontalMovement.y > 0)
                input.y = -Time.deltaTime * _filter2;
            else
                if (_horizontalMovement.y < 0)
                input.y = Time.deltaTime * _filter2;

            if (_horizontalMovement.x > 0)
                input.x = -Time.deltaTime * _filter2;
            else
                input.x = Time.deltaTime * _filter2;

            if (Vector3.Distance(new Vector3(_targetPoint.Position.x, 0, _targetPoint.Position.z), new Vector3(transform.position.x, 0, transform.position.z)) > 10)
            {
                Vector2 movementDireation = new Vector2(_targetPoint.Position.x - transform.position.x, _targetPoint.Position.z - transform.position.z).normalized;

                Vector2 forward;
                Vector2 rightward;

                forward = new Vector2(transform.forward.x, transform.forward.z);
                rightward = new Vector2(transform.right.x, transform.right.z);

                _forward = forward;
                _rightWar = rightward;

                float forwardProject = Project(movementDireation, forward);
                float rightwardProject = Project(movementDireation, rightward);

                rightwardProject = Mathf.Abs(rightwardProject) < 0.4 ? 0 : rightwardProject;

                input = new Vector2(rightwardProject, forwardProject);
            }
            else if (_lookToTarget)
            {
                Vector3 targetDirection = _playerTank.transform.position - transform.position;
                Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

                targetDirection.y = 0;
                targetDirection.Normalize();

                

                float rotationDirection = Vector3.Cross(forward, targetDirection).y * TurnForce;

                //rotationDirection = rotationDirection < 0 ? -1 : 1;

                rotationDirection = Mathf.Max(Mathf.Abs(rotationDirection), 8 * Mathf.Min(1, Vector3.Angle(forward, targetDirection) / 45)) * (rotationDirection < 0 ? -1 : 1);

                float force = rotationDirection * (turnForcePercent - Mathf.Abs(_horizontalMovement.y)) * _rigidbody.mass;

                _rigidbody.AddRelativeTorque(0f, force, 0, _forceMode);
                //Debug.Log(_horizontalMovement);
            }

            if (Vector3.Distance(new Vector3(0, _targetPoint.Position.y, 0), new Vector3(0, transform.position.y, 0)) > 3)
            {
                float delta = Vector3.Distance(new Vector3(0, _targetPoint.Position.y, 0), new Vector3(0, transform.position.y, 0)) / 20;
                delta = Mathf.Clamp(delta, 0, 1);

                _engineForce += (_targetPoint.Position - transform.position).y * _heightSpeed * delta * Time.deltaTime;

                _engineForce = Mathf.Clamp(_engineForce, 10, 30);
            }

            _horizontalMovement.x += input.x;
            _horizontalMovement.x = Mathf.Clamp(_horizontalMovement.x, -1, 1);

            _horizontalMovement.y += input.y;
            _horizontalMovement.y = Mathf.Clamp(_horizontalMovement.y, -1, 1);
        }

        public void Initialize(HelicopterPointConfig helicopterPointConfig)
        {
            _path = helicopterPointConfig.Path;

            _targetPoint = _path[0];
        }

        private void MoveProcess()
        {
            float turn = TurnForce * Mathf.Lerp(_horizontalMovement.x, _horizontalMovement.x * (turnTiltForcePercent - Mathf.Abs(_horizontalMovement.y)), Mathf.Max(0f, _horizontalMovement.y));

            _horizontalTurnAngle = Mathf.Lerp(_horizontalTurnAngle, turn, Time.fixedDeltaTime * TurnForce);
            _rigidbody.AddRelativeTorque(0f, _horizontalTurnAngle * _rigidbody.mass, 0f);

            _rigidbody.AddRelativeForce(Vector3.forward * Mathf.Max(0f, _horizontalMovement.y * ForwardForce * _rigidbody.mass));
        }

        private void LiftProcess()
        {
            var upForce = 1 - Mathf.Clamp(_rigidbody.transform.position.y / EffectiveHeight, 0, 1);
            upForce = Mathf.Lerp(0f, _engineForce, upForce) * _rigidbody.mass;
            _rigidbody.AddRelativeForce(Vector3.up * upForce);
        }

        private void TiltProcess()
        {
            _hotizontalTilt.x = Mathf.Lerp(_hotizontalTilt.x, _horizontalMovement.x * TurnTiltForce, Time.deltaTime);
            _hotizontalTilt.y = Mathf.Lerp(_hotizontalTilt.y, _horizontalMovement.y * ForwardTiltForce, Time.deltaTime);
            _rigidbody.transform.localRotation = Quaternion.Euler(_hotizontalTilt.y, _rigidbody.transform.localEulerAngles.y, -_hotizontalTilt.x);
        }

        private float Project(Vector2 vector, Vector2 axis)
        {
            float scalarProduct = (vector.x * axis.x) + (vector.y * axis.y);
            float axisModul = Mathf.Sqrt(Mathf.Pow(axis.x, 2) + Mathf.Pow(axis.y, 2));
            float project = scalarProduct / axisModul;

            return project;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawLine(transform.position, transform.position + new Vector3(_forward.x, 0, _forward.y) * 10);

            Gizmos.color = Color.red;

            Gizmos.DrawLine(transform.position, transform.position + new Vector3(_rightWar.x, 0, _rightWar.y) * 10);
        }

        private IEnumerator Mover()
        {
            bool isMoved = true;

            yield return new WaitForSeconds(10);

            while (isMoved)
            {
                _targetPoint = _currentPointIndex < _path.Length - 1 ? _path[_currentPointIndex + 1] : _path[0];

                while (Vector3.Distance(transform.position, _targetPoint.Position) > 10)
                    yield return null;

                if (_lookToTarget)
                    yield return new WaitForSeconds(_waitingTime);

                _currentPointIndex++;

                if (_currentPointIndex >= _path.Length)
                    _currentPointIndex = 0;

                yield return null;
            }
        }
    }
}
