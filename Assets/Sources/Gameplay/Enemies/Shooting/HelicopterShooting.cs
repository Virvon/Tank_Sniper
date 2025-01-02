using UnityEngine;

namespace Assets.Sources.Gameplay.Weapons
{
    public class HelicopterShooting : EnemyShooting
    {
        [SerializeField] private Transform[] _bulletPoints;
        [SerializeField] private Transform _helicopter;

        private int _currentShootPointIndex;
        protected override bool CanShoot => base.CanShoot && Vector2.Angle(new Vector2(PlayerTankWrapper.transform.position.x - LookStartPosition.x, PlayerTankWrapper.transform.position.z - LookStartPosition.z), new Vector2(_helicopter.transform.forward.x, _helicopter.transform.forward.z)) < AngleDelta;
        protected override Vector3 LookStartPosition => transform.position;

        protected override Vector3 GetCurrentShootPointPosition()
        {
            Vector3 currentShootPointPosition = _bulletPoints[_currentShootPointIndex].position;

            _currentShootPointIndex = _currentShootPointIndex >= _bulletPoints.Length - 1 ? 0 : _currentShootPointIndex + 1;

            return currentShootPointPosition;
        }
    }
}