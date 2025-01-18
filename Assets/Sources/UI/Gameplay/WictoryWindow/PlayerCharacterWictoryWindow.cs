using Assets.Sources.Gameplay.Cameras;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Gameplay.WictoryWindow
{
    public class PlayerCharacterWictoryWindow : WicotoryWindow
    {
        [SerializeField] private PlayerCharacterRewardPanel _playerCharacterRewardPanel;
        [SerializeField] private CanvasGroup _continueButtonCanvasGroup;
        [SerializeField] private float _continueButtonShowDuration;
        [SerializeField] private Canvas _canvas;

        [Inject]
        private void Construct(UiCamera uiCamera)
        {
            _canvas.worldCamera = uiCamera.Camera;

            SetContinueButtonActive(false);
        }

        protected override async void OnWindowsSwitched()
        {
            await _playerCharacterRewardPanel.GenerateCharacter();
            base.OnWindowsSwitched();

            StartCoroutine(ContinueButtonShower());
        }

        private void SetContinueButtonActive(bool isActive)
        {
            _continueButtonCanvasGroup.alpha = isActive ? 1 : 0;
            _continueButtonCanvasGroup.interactable = isActive;
            _continueButtonCanvasGroup.blocksRaycasts = isActive;
        }

        private IEnumerator ContinueButtonShower()
        {
            yield return new WaitForSeconds(_continueButtonShowDuration);

            SetContinueButtonActive(true);
        }
    }
}
