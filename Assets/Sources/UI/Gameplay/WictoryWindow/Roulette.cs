using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Gameplay.WictoryWindow
{
    public class Roulette : MonoBehaviour
    {
        [SerializeField] private Transform _pointer;
        [SerializeField] private float _minRotation;
        [SerializeField] private float _maxRotation;
        [SerializeField] private TMP_Text _rewardValue;
        [SerializeField] private RewartInfo[] _rewards;
        [SerializeField] private Button _button;

        private AnimationsConfig _animationsConfig;

        private uint _startReward;
        private uint _reward;
        private bool _isRotated;

        public event Action<uint> Rewarded;

        [Inject]
        private void Construct(IStaticDataService staticDataService)
        {
            _animationsConfig = staticDataService.AnimationsConfig;

            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnButtonClicked);

        private void OnButtonClicked()
        {
            _button.interactable = false;
            _isRotated = false;

#if !UNITY_WEBGL || UNITY_EDITOR
            Rewarded?.Invoke(_reward);
#else
            Agava.YandexGames.InterstitialAd.Show(onCloseCallback: (value) =>
            {
                Rewarded?.Invoke(_reward);
            });
#endif
        }

        public void Work(uint reward)
        {
            _startReward = reward;

            StartCoroutine(Rotater());
        }

        private uint CalculateReward(float rotationProgress)
        {
            uint modifier = 1;

            RewartInfo[] rewards = _rewards.OrderBy(reward => reward.Modifier).ToArray();

            foreach(RewartInfo rewardInfo in rewards)
            {
                if (rotationProgress <= rewardInfo.RotationProgress)
                {
                    modifier = rewardInfo.Modifier;
                    break;
                }
            }

            return _startReward * modifier;
        }

        private IEnumerator Rotater()
        {
            float progress;
            float passedTime = 0;
            float duration = _animationsConfig.RouletteRotateDuration;
            AnimationCurve animationCurve = _animationsConfig.RouletteAnimationCurve;
            _isRotated = true;

            while (_isRotated)
            {
                progress = Mathf.Clamp(passedTime / duration, 0, 1);
                passedTime += Time.deltaTime;

                if (passedTime > duration)
                    passedTime = 0;

                float rotationProgress = animationCurve.Evaluate(progress);
                float rotation = Mathf.Lerp(_minRotation, _maxRotation, rotationProgress);
                _pointer.transform.rotation = Quaternion.Euler(0, 0, rotation);
                _reward = CalculateReward(rotationProgress);
                _rewardValue.text = "+ " + _reward.ToString();

                yield return null;
            }
        }
    }
}
