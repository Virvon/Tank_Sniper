using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.Enemies;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Bullets
{
    public class HomingBullet : CollidingBullet
    {
        private const float Offset = 1;

        private GameplayCamera _gameplayCamera;

        private uint _rotationSpeed;
        private float _targetingDelay;
        private uint _searchRadius;

        protected Transform Target;
        private bool _isFollowed;

        [Inject]
        private void Construct(GameplayCamera gameplayCamera) =>
            _gameplayCamera = gameplayCamera;

        private void Start() =>
            SearchTarget(_searchRadius);

        public HomingBullet BindHomingSettings(uint searchRadius, uint rotationSpeed, float targetingDelay)
        {
            _rotationSpeed = rotationSpeed;
            _targetingDelay = targetingDelay;
            _searchRadius = searchRadius;

            _isFollowed = false;

            return this;
        }

        protected override void Explode()
        {
            base.Explode();
            _isFollowed = false;
        }

        protected virtual void SearchTarget(uint searchRadius)
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();

            Vector3 center = _gameplayCamera.Camera.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, 0));

            enemies = enemies.Where(enemy => Vector3.Distance(center, _gameplayCamera.Camera.WorldToScreenPoint(new Vector3(enemy.transform.position.x, enemy.transform.position.y, 1))) <= searchRadius).ToArray();

            if (enemies.Length > 0)
            {
                Target = enemies[Random.Range(0, enemies.Length)].transform;

                StartCoroutine(Follower());
            }
        }

        protected IEnumerator Follower()
        {
            _isFollowed = true;

            yield return new WaitForSeconds(_targetingDelay);

            while (_isFollowed)
            {
                Quaternion targetRotation = Quaternion.LookRotation((Target.transform.position + (Vector3.up * Offset)  - transform.position).normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
                ChangeTrajectory();

                yield return null;
            }
        }
    }
}
