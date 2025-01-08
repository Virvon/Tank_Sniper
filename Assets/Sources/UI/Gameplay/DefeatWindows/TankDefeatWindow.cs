using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using MPUIKIT;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Gameplay.DefeatWindows
{
    public class TankDefeatWindow : DefeatWindow
    {
        [SerializeField] private TMP_Text _timerValue;
        [SerializeField] private MPImage _timerFill;
        
        private GameplaySettingsConfig _gameplaySettings;

        [Inject]
        private void Construct(IStaticDataService staticDataService) =>
            _gameplaySettings = staticDataService.GameplaySettingsConfig;

        protected override void OnWindowsSwitched()
        {
            base.OnWindowsSwitched();
            StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            float progressRecoveryAvailableTime = _gameplaySettings.ProgressRecoveryAvailableTime;
            float maxAvailableTime = progressRecoveryAvailableTime;
            float passedTime = 0;

            while (progressRecoveryAvailableTime > 0)
            {
                passedTime += Time.deltaTime;
                progressRecoveryAvailableTime -= Time.deltaTime;
                int abcProgressRecoveryAvailableTime = (int)maxAvailableTime - (int)passedTime;

                _timerValue.text = abcProgressRecoveryAvailableTime.ToString();
                _timerFill.fillAmount = progressRecoveryAvailableTime / maxAvailableTime;

                yield return null;
            }

            ProgressRecoveryButton.interactable = false;
        }
    }
}
