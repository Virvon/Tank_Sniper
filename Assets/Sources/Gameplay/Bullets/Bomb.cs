using System;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Bullets
{
    public class Bomb : CollidingBullet
    {
        private bool _canCollide;

        private void Start()
        {
            _canCollide = false;

            StartCoroutine(Waiter(0.1f, () => _canCollide = true));
        }

        protected override void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.TryGetComponent(out IDamageable _) && _canCollide)
                Explode();
        }

        protected override void DestroyAfterLifeTimeLimit(float lifeTimeLimt)
        {
            StartCoroutine(Waiter(lifeTimeLimt, () => Explode()));
        }

        private IEnumerator Waiter(float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);

            callback?.Invoke();
        }
    }
}
