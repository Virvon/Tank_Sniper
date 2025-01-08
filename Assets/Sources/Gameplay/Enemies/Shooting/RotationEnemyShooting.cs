using Assets.Sources.Gameplay.Player;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Weapons
{
    public class RotationEnemyShooting : EnemyForwartFlyingBulletsShooting
    {
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _rotationSpeed;

        private bool _isRotated;
        private bool _isTurnedToPlayerTank;

        protected override bool CanShoot => base.CanShoot && _isTurnedToPlayerTank;

        protected override Vector3 LookStartPosition => _shootPoint.position;

        private void Start()
        {
            _isRotated = false;
            _isTurnedToPlayerTank = false;
        }

        protected override void StartShooting()
        {
            base.StartShooting();
            StartCoroutine(Rotater());
        }

        protected override void OnEnemyDestructed()
        {
            base.OnEnemyDestructed();
            _isRotated = false;
        }

        protected override Vector3 GetCurrentShootPointPosition() =>
            _shootPoint.position;

        private IEnumerator Rotater()
        {
            _isRotated = true;

            while (_isRotated)
            {
                Vector3 shootPointForward = _shootPoint.forward;
                Vector3 targetDirection = (PlayerWrapper.transform.position - _shootPoint.position).normalized;

                Quaternion targetRotation = Quaternion.Euler(
                0,
                transform.rotation.eulerAngles.y + Quaternion.FromToRotation(shootPointForward, targetDirection).eulerAngles.y,
                0);

                shootPointForward.y = 0;
                targetDirection.y = 0;

                if (Vector3.Angle(shootPointForward, targetDirection) > AngleDelta)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
                    _isTurnedToPlayerTank = false;
                }
                else
                {
                    _isTurnedToPlayerTank = true;
                }

                yield return null;
            }
        }
    }
}
