using Assets.Sources.Services.StaticDataService.Configs;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.MainMenu.Animations
{
    public class TankScalingAnimator : TankAnimator
    {
        private const float StartScale = 1;

        protected override IEnumerator Animator()
        {
            float progress = 0;
            float passedTime = 0;

            while (progress < 1)
            {
                passedTime += Time.deltaTime;
                progress = passedTime / AnimationsConfig.TankScalingDuration;

                transform.localScale = new Vector3(StartScale, StartScale * AnimationsConfig.TankScalingAnimationCurve.Evaluate(progress), StartScale);

                yield return null;
            }
        }
    }
}
