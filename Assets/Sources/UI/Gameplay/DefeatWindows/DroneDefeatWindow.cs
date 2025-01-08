using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Gameplay.DefeatWindows
{
    public class DroneDefeatWindow : DefeatWindow
    {
        [SerializeField] private TMP_Text _progressRecoveryRewarValue;

        private bool _isButtonAnimated;
        private AnimationsConfig _animationConfig;

        [Inject]
        private void Construct(uint dronesCount, IStaticDataService staticDataService)
        {
            _progressRecoveryRewarValue.text = $"+{dronesCount}";
            _animationConfig = staticDataService.AnimationsConfig;
        }

        protected override void OnWindowsSwitched()
        {
            base.OnWindowsSwitched();
            StartCoroutine(Animator());
        }

        protected override void OnProgressRecovery()
        {
            base.OnProgressRecovery();
            _isButtonAnimated = false;
        }

        private IEnumerator Animator()
        {
            float progress;
            float passedTime = 0;
            float duration = _animationConfig.DroneDefeateRecoveryProgressButtonAnimationDuration;
            AnimationCurve animationCurve = _animationConfig.DroneDefeatRecoveryProgressButtonAnimationCurve;

            _isButtonAnimated = true;

            while (_isButtonAnimated)
            {
                progress = passedTime / duration;
                passedTime += Time.deltaTime;

                float scale = animationCurve.Evaluate(progress);
                ProgressRecoveryButton.transform.localScale = Vector3.one * scale;

                if (passedTime >= duration)
                    passedTime = 0;

                yield return null;
            }

            ProgressRecoveryButton.transform.localScale = Vector3.one;
        }
    }
}
