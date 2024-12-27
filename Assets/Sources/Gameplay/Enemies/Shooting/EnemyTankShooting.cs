using Assets.Sources.Gameplay.Player;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Weapons
{
    public class EnemyTankShooting : EnemyShooting
    {
        [SerializeField] private Transform _turret;
        [SerializeField] private uint _turretRotationSpeed;

        private bool _isTurretRotated;
        private bool _isTurnedToPlayerTank;

        protected override bool CanShoot => base.CanShoot && _isTurnedToPlayerTank;

        private void Start()
        {
            _isTurnedToPlayerTank = false;
            _isTurretRotated = false;
        }

        protected override void StartShooting()
        {
            base.StartShooting();
            StartCoroutine(TurretRotater());
        }

        protected override void OnEnemyDestructed()
        {
            base.OnEnemyDestructed();
            _isTurretRotated = false;
        }

        private IEnumerator TurretRotater()
        {
            _isTurretRotated = true;

            while (_isTurretRotated)
            {
                Vector3 targetDiretion = (PlayerTankWrapper.transform.position - _turret.transform.position).normalized;

                Quaternion targetRotation = _turret.transform.rotation * Quaternion.AngleAxis(Quaternion.LookRotation(targetDiretion).eulerAngles.x, Vector3.right);

                if(Vector3.Angle(_turret.transform.forward, targetDiretion) > AngleDelta)
                {
                    _turret.transform.rotation = Quaternion.RotateTowards(_turret.transform.rotation, targetRotation, _turretRotationSpeed * Time.deltaTime);
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
