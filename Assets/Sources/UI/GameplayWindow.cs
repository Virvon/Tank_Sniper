using Assets.Sources.Gameplay;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI
{
    public class GameplayWindow : Window
    {
        [SerializeField] private CanvasGroup _overviewAimCanvasGroup;
        [SerializeField] private CanvasGroup _aimButtonCanvasGroup;
        [SerializeField] private CanvasGroup _aimingCanvasGroup;

        private Aiming _aiming;

        private Coroutine _aimChanger;

        [Inject]
        private void Construct(Aiming aiming)
        {
            _aiming = aiming;

            _aiming.StateChanged += OnAimingStateChanged;
            _aiming.StateChangingFinished += OnAimingStageChangingFinished;
        }

        private void OnDestroy()
        {
            _aiming.StateChanged -= OnAimingStateChanged;
            _aiming.StateChangingFinished -= OnAimingStageChangingFinished;
        }

        private void OnAimingStageChangingFinished(bool isAimed)
        {
            if(isAimed == false)
            {
                _aimButtonCanvasGroup.alpha = 1;
                _aimButtonCanvasGroup.blocksRaycasts = true;
                _aimButtonCanvasGroup.interactable = true;
            }
        }

        private void OnAimingStateChanged(bool isAimed, float duration)
        {
            if (isAimed)
            {
                _overviewAimCanvasGroup.alpha = 0;
                _aimButtonCanvasGroup.alpha = 0;
                _aimButtonCanvasGroup.blocksRaycasts = false;
                _aimButtonCanvasGroup.interactable = false;
            }
            else
            {
                _overviewAimCanvasGroup.alpha = 1;
            }

            if (_aimChanger != null)
                StopCoroutine(_aimChanger);

            _aimChanger = StartCoroutine(AimChanger(isAimed, duration));
        }

        private IEnumerator AimChanger(bool isAimed, float duration)
        {
            float progress;
            float passedTime = 0;
            float targetAlpha = isAimed ? 1 : 0;
            float startAlpha = _aimingCanvasGroup.alpha;

            while(_aimingCanvasGroup.alpha != targetAlpha)
            {
                progress = passedTime / duration;
                passedTime += Time.deltaTime;

                _aimingCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, progress);

                yield return null;
            }
        }
    }
}
