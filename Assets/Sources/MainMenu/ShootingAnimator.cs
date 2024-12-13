using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.MainMenu
{
    public class ShootingAnimator : TankAnimator
    {
        

        private Quaternion _startRotation;
        private Quaternion _shootingRotation;

        

        private void Start()
        {
            _startRotation = transform.rotation;
            _shootingRotation = _startRotation * Quaternion.AngleAxis(AnimationsConfig.TankShootingRotationAngle, -Vector3.right);
        }

        protected override IEnumerator Animator()
        {
            float progress = 0;
            float passedTime = 0;

            while(progress < 1)
            {
                passedTime += Time.deltaTime;
                progress = passedTime / AnimationsConfig.TankShootingDuration;

                transform.rotation = Quaternion.Lerp(_startRotation, _shootingRotation, AnimationsConfig.TankShootingAnimationCurve.Evaluate(progress));

                yield return null;
            }
        }
    }

    public abstract class TankAnimator : MonoBehaviour
    {
        private Coroutine _animator;

        public AnimationsConfig AnimationsConfig { get; private set; }

        [Inject]
        private void Construct(IStaticDataService staticDataService) =>
            AnimationsConfig = staticDataService.AnimationsConfig;

        public void Play()
        {
            if (_animator != null)
                StopCoroutine(_animator);

            _animator = StartCoroutine(Animator());
        }

        protected abstract IEnumerator Animator();
    }
}
