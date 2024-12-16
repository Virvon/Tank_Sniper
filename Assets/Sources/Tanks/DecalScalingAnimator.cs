using Assets.Sources.MainMenu;
using Assets.Sources.Services.StaticDataService.Configs;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Tanks
{
    public class DecalScalingAnimator : TankAnimator
    {
        private const float StartScale = 1;

        protected override IEnumerator Animator()
        {
            float progress = 0;
            float passedTime = 0;

            while (progress < 1)
            {
                passedTime += Time.deltaTime;
                progress = passedTime / AnimationsConfig.DecalScalingDuration;

                transform.localScale = Vector3.one * StartScale * AnimationsConfig.DecalScalingAnimationCurve.Evaluate(progress);

                yield return null;
            }
        }
    }
}
