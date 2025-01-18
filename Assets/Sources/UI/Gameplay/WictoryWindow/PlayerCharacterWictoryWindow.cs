using Assets.Sources.Gameplay.Cameras;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Assets.Sources.UI.Gameplay.WictoryWindow
{
    public class PlayerCharacterWictoryWindow : WicotoryWindow
    {
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private PlayerCharacterRewardPanel _playerCharacterRewardPanel;
        [SerializeField] private CanvasGroup _continueButtonCanvasGroup;
        [SerializeField] private float _continueButtonShowDuration;

        [Inject]
        private void Construct(GameplayCamera gameplayCamera)
        {
            UniversalAdditionalCameraData cameraData = gameplayCamera.Camera.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Add(_uiCamera);

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
