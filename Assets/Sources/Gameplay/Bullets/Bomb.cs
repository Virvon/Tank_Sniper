using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.Bullets
{
    public class Bomb : CollidingBullet
    {
        protected override void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.TryGetComponent(out IDamageable _))
                Explode();
        }

        protected override void DestroyAfterLifeTimeLimit(float lifeTimeLimt) =>
            Explode();
    }
}
