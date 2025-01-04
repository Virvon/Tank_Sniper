using Assets.Sources.Gameplay.Handlers;
using Assets.Sources.Gameplay.Player;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Gameplay
{
    public class GameplayWindow : OpenableWindow
    {
        [SerializeField] private CanvasGroup _overviewAimCanvasGroup;
        [SerializeField] private CanvasGroup _aimButtonCanvasGroup;
        [SerializeField] private CanvasGroup _aimingCanvasGroup;

        private Aiming _aiming;
        private DefeatHandler _defeatHandler;

        private Coroutine _aimChanger;
        private bool _isDefeated;

        [Inject]
        private void Construct(Aiming aiming, DefeatHandler defeatHandler)
        {
            _aiming = aiming;
            _defeatHandler = defeatHandler;

            _isDefeated = false;

            _aiming.StateChanged += OnAimingStateChanged;
            _aiming.StateChangingFinished += OnAimingStageChangingFinished;
            _defeatHandler.Defeated += OnDefeat;
            _defeatHandler.WindowsSwitched += OnWindowsSwithced;
            _defeatHandler.ProgressRecovery += OnProgressRecovery;
        }

        private void OnDestroy()
        {
            _aiming.StateChanged -= OnAimingStateChanged;
            _aiming.StateChangingFinished -= OnAimingStageChangingFinished;
            _defeatHandler.Defeated -= OnDefeat;
            _defeatHandler.WindowsSwitched -= OnWindowsSwithced;
            _defeatHandler.ProgressRecovery -= OnProgressRecovery;
        }

        private void OnAimingStageChangingFinished(bool isAimed)
        {
            if (isAimed == false)
                SetAimButtonActive(true);
        }

        private void OnProgressRecovery()
        {
            _isDefeated = false;
            SetAimButtonActive(true);
            Open();
        }

        private void OnDefeat() =>
            _isDefeated = true;

        private void OnWindowsSwithced() =>
            Close();

        private void SetAimButtonActive(bool isActive)
        {
            isActive = _isDefeated ? false : isActive;
            _aimButtonCanvasGroup.alpha = isActive ? 1 : 0;
            _aimButtonCanvasGroup.blocksRaycasts = isActive;
            _aimButtonCanvasGroup.interactable = isActive;
        }

        private void OnAimingStateChanged(bool isAimed, float duration)
        {
            if (isAimed)
            {
                _overviewAimCanvasGroup.alpha = 0;
                SetAimButtonActive(false);
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

            while (_aimingCanvasGroup.alpha != targetAlpha)
            {
                progress = passedTime / duration;
                passedTime += Time.deltaTime;

                _aimingCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, progress);

                yield return null;
            }
        }
    }
}