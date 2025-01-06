﻿using Assets.Sources.Gameplay.Enemies;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Weapons
{
    public class EnemyRobotShooting : EnemyShooting
    {
        private const float AngleDelta = 6;
        private const float ChangingRotationDuration = 0.4f;
        private const float LaserRotationSpeed = 10;
        private const float RaycasDistance = 300;

        [SerializeField] private GameObject _laserPrefab;
        [SerializeField] private uint _damgagePerTime;
        [SerializeField] private float _damageCooldown;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private uint _explosionForce;

        private GameObject _laser;
        private Quaternion _targetRotation;

        protected override Vector3 GetCurrentShootingPosition() =>
            _shootPoint.position;

        protected override IEnumerator Shooter()
        {
            float attackPassedTime = 0;
            float diretionChangingPassedTime = 0;

            while (IsShooted)
            {
                Vector3 shootPointForward = _shootPoint.forward;
                Vector3 targetDirection = (PlayerTankWrapper.transform.position - _shootPoint.position).normalized;

                shootPointForward.y = 0;
                targetDirection.y = 0;

                if(Vector3.Angle(shootPointForward, targetDirection) < AngleDelta)
                {
                    diretionChangingPassedTime += Time.deltaTime;
                    attackPassedTime += Time.deltaTime;

                    if (_laser == null)
                        _laser = Instantiate(_laserPrefab, _shootPoint.position, GetShootingRotation(), _shootPoint.transform);
                    else
                        _laser.transform.rotation = Quaternion.RotateTowards(_laser.transform.rotation, _targetRotation, LaserRotationSpeed * Time.deltaTime);

                    if (diretionChangingPassedTime >= ChangingRotationDuration)
                    {
                        diretionChangingPassedTime = 0;
                        _targetRotation = GetShootingRotation();
                    }

                    if (attackPassedTime >= _damageCooldown)
                    {
                        attackPassedTime = 0;

                        if (Physics.Raycast(GetCurrentShootingPosition(), _laser.transform.forward, out RaycastHit hitInfo, RaycasDistance)
                            && hitInfo.transform.TryGetComponent(out IDamageable damageable) && damageable is not EnemyRobot)
                            damageable.TakeDamage(new ExplosionInfo(hitInfo.point, _explosionForce, true, _damgagePerTime));
                    }
                }
                else if(_laser != null)
                {
                    Destroy(_laser);
                    _laser = null;
                }

                yield return null;
            }
        }
    }
}