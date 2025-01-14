using Assets.Sources.Gameplay.Player;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Weapons
{
    public class EnemyTankShooting : RotationEnemyShooting
    {
        [SerializeField] private Transform _turret;
        [SerializeField] private uint _turretRotationSpeed;

        private bool _isTurretRotated;
        private bool _isTurretTurnedToPlayerTank;

        protected override bool CanShoot => base.CanShoot && _isTurretTurnedToPlayerTank;

        private void Start()
        {
            _isTurretTurnedToPlayerTank = false;
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
                Vector3 targetDiretion = (PlayerWrapper.transform.position - _turret.transform.position).normalized;

                Quaternion targetRotation = Quaternion.LookRotation(targetDiretion, Vector3.right);

                if(Vector3.Angle(_turret.transform.forward, targetDiretion) > AngleDelta)
                {
                    _turret.transform.rotation = Quaternion.RotateTowards(_turret.transform.rotation, targetRotation, _turretRotationSpeed * Time.deltaTime);
                    _isTurretTurnedToPlayerTank = false;
                }
                else
                {
                    _isTurretTurnedToPlayerTank = true;
                }

                yield return null;
            }
        }
    }
}
