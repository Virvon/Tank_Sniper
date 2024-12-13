using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.MainMenu
{
    public class ShootingAnimator : MonoBehaviour
    {
        private AnimationsConfig _animationsConfig;

        private Quaternion _startRotation;
        private Quaternion _shootingRotation;
        private Coroutine _animator;

        [Inject]
        private void Construct(IStaticDataService staticDataService)
        {
            _animationsConfig = staticDataService.AnimationsConfig;
        }

        private void Start()
        {
            _startRotation = transform.rotation;
            _shootingRotation = _startRotation * Quaternion.AngleAxis(_animationsConfig.TankShootingRotationAngle, -Vector3.right);
        }

        public void Play()
        {
            if (_animator != null)
                StopCoroutine(_animator);

            _animator = StartCoroutine(Animator());
        }

        private IEnumerator Animator()
        {
            float progress = 0;
            float passedTime = 0;

            while(progress < 1)
            {
                passedTime += Time.deltaTime;
                progress = passedTime / _animationsConfig.TankShootingDuration;

                transform.rotation = Quaternion.Lerp(_startRotation, _shootingRotation, _animationsConfig.TankShootingAnimationCurve.Evaluate(progress));

                yield return null;
            }
        }
    }
}
