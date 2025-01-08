using Assets.Sources.Gameplay.Handlers;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Gameplay.GameplayWindows
{
    public class GameplayWindow : OpenableWindow
    {
        [SerializeField] private CanvasGroup _overviewAimCanvasGroup;
        [SerializeField] private CanvasGroup _aimButtonCanvasGroup;
        [SerializeField] private CanvasGroup _aimingCanvasGroup;

        private DefeatHandler _defeatHandler;
        private WictoryHandler _wictoryHangler;

        private bool _isAimButtonHided;

        protected CanvasGroup OverviewAimCanvasGroup => _overviewAimCanvasGroup;
        protected CanvasGroup AimingCanvasGroup => _aimingCanvasGroup;

        [Inject]
        private void Construct(DefeatHandler defeatHandler, WictoryHandler wictoryHandler)
        {
            _defeatHandler = defeatHandler;
            _wictoryHangler = wictoryHandler;

            _isAimButtonHided = false;

            _defeatHandler.Defeated += HideAimButton;
            _defeatHandler.WindowsSwitched += OnWindowsSwithced;
            _defeatHandler.ProgressRecovered += OnProgressRecovery;
            _wictoryHangler.WindowsSwithed += OnWindowsSwithced;
            _wictoryHangler.Woned += HideAimButton;
        }

        protected virtual void OnDestroy()
        {
            _defeatHandler.Defeated -= HideAimButton;
            _defeatHandler.WindowsSwitched -= OnWindowsSwithced;
            _defeatHandler.ProgressRecovered -= OnProgressRecovery;
            _wictoryHangler.WindowsSwithed -= OnWindowsSwithced;
            _wictoryHangler.Woned -= HideAimButton;
        }

        private void OnProgressRecovery()
        {
            _isAimButtonHided = false;
            SetAimButtonActive(true);
            Show();
        }

        private void HideAimButton() =>
            _isAimButtonHided = true;

        private void OnWindowsSwithced() =>
            Hide();

        protected void SetAimButtonActive(bool isActive)
        {
            isActive = _isAimButtonHided ? false : isActive;
            _aimButtonCanvasGroup.alpha = isActive ? 1 : 0;
            _aimButtonCanvasGroup.blocksRaycasts = isActive;
            _aimButtonCanvasGroup.interactable = isActive;
        }
    }
}