using Assets.Sources.Gameplay.Enemies;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.Bullets
{
    public class HomingBullet : CollidingBullet
    {
        private GameplayCamera _gameplayCamera;

        private uint _rotationSpeed;
        private float _targetingDelay;

        private Enemy _target;
        private bool _isFollowed;

        [Inject]
        private void Construct(GameplayCamera gameplayCamera) =>
            _gameplayCamera = gameplayCamera;

        public HomingBullet BindHomingSettings(uint searchRadius, uint rotationSpeed, float targetingDelay)
        {
            _rotationSpeed = rotationSpeed;
            _targetingDelay = targetingDelay;

            _isFollowed = false;

            SearchTarget(searchRadius);

            return this;
        }

        protected override void Explode()
        {
            base.Explode();
            _isFollowed = false;
        }

        private void SearchTarget(uint searchRadius)
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();

            Vector3 center = _gameplayCamera.Camera.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, 0));

            enemies = enemies.Where(enemy => Vector3.Distance(center, _gameplayCamera.Camera.WorldToScreenPoint(new Vector3(enemy.transform.position.x, enemy.transform.position.y, 1))) <= searchRadius).ToArray();

            if (enemies.Length > 0)
            {
                _target = enemies[Random.Range(0, enemies.Length)];

                StartCoroutine(Follower());
            }
        }

        private IEnumerator Follower()
        {
            _isFollowed = true;

            yield return new WaitForSeconds(_targetingDelay);

            while (_isFollowed)
            {
                Quaternion targetRotation = transform.rotation * Quaternion.FromToRotation(transform.forward, (_target.transform.position - transform.position).normalized);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
                ChangeTrajectory();

                yield return null;
            }
        }
    }
}
