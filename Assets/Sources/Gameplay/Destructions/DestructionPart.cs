using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Destructions
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class DestructionPart : MonoBehaviour
    {
        private const string DestructLayer = "IgnoreProjectile";

        private DestructionConfig _destructionConfig;

        private Rigidbody _rigidbody;

        [Inject]
        private void Construct(IStaticDataService staticDataService)
        {
            _destructionConfig = staticDataService.DestructionConfig;

            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
        }

        public virtual void Destruct(Vector3 bulletPosition, uint explosionForce)
        {
            gameObject.layer = LayerMask.NameToLayer(DestructLayer);

            Vector3 explosionDirection = (transform.position - bulletPosition).normalized;
            explosionDirection += _destructionConfig.AdditionalDestructionDirection;
            explosionDirection.Normalize();

            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(explosionDirection * explosionForce, ForceMode.Impulse);
            _rigidbody.AddTorque(explosionDirection * _destructionConfig.RotationForce, ForceMode.Impulse);

            StartCoroutine(Destroyer());
        }

        private IEnumerator Destroyer()
        {
            yield return new WaitForSeconds(_destructionConfig.DestroyDelay);

            Vector3 targetScale = Vector3.zero;
            Vector3 startScale = transform.localScale;
            float passedTime = 0;
            float progress;

            while (transform.localScale != targetScale)
            {
                progress = passedTime / _destructionConfig.DestroyDuration;
                passedTime += Time.deltaTime;

                transform.localScale = Vector3.Lerp(startScale, targetScale, progress);

                yield return null;
            }

            Destroy(gameObject);
        }
    }
}