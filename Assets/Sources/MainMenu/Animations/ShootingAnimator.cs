using Assets.Sources.Services.StaticDataService.Configs;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.MainMenu.Animations
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

            while (progress < 1)
            {
                passedTime += Time.deltaTime;
                progress = passedTime / AnimationsConfig.TankShootingDuration;

                transform.rotation = Quaternion.Lerp(_startRotation, _shootingRotation, AnimationsConfig.TankShootingAnimationCurve.Evaluate(progress));

                yield return null;
            }
        }
    }
}
