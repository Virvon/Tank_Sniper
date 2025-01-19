using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Destructions
{
    public class DestructionPart : MonoBehaviour
    {
        [SerializeField] private bool _isDestroyedImmediately;

        private const string DestructLayer = "IgnoreProjectile";

        private DestructionConfig _destructionConfig;

        private Rigidbody _rigidbody;

        protected virtual bool IsIgnoreRotation => false;

        [Inject]
        private void Construct(IStaticDataService staticDataService)
        {
            _destructionConfig = staticDataService.DestructionConfig;

            _rigidbody = GetDestructionRigidbody();
            _rigidbody.isKinematic = true;
        }

        public virtual void Destruct(Vector3 explosionPosition, uint explosionForce)
        {
            gameObject.layer = LayerMask.NameToLayer(DestructLayer);

            Vector3 explosionDirection = (transform.position - explosionPosition).normalized;
            explosionDirection += _destructionConfig.AdditionalDestructionDirection;
            explosionDirection.Normalize();

            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(explosionDirection * explosionForce, ForceMode.Impulse);
            
            if(IsIgnoreRotation == false)
                _rigidbody.AddTorque(explosionDirection * _destructionConfig.RotationForce, ForceMode.Impulse);

            StartCoroutine(Destroyer());
        }

        protected virtual Rigidbody GetDestructionRigidbody() =>
            GetComponent<Rigidbody>();

        private IEnumerator Destroyer()
        {
            yield return new WaitForSeconds(_destructionConfig.DestroyDelay);

            if (_isDestroyedImmediately)
                Destroy(gameObject);

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