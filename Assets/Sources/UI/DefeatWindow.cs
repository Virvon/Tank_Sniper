using Assets.Sources.Gameplay;
using Assets.Sources.Infrastructure.GameStateMachine;
using Assets.Sources.Infrastructure.GameStateMachine.States;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;
using MPUIKIT;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI
{
    public class DefeatWindow : OpenableWindow
    {
        [SerializeField] private TMP_Text _timerValue;
        [SerializeField] private MPImage _timerFill;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _progressRecoveryButton;

        private DefeatHandler _defeatHandler;
        private GameplaySettingsConfig _gameplaySettings;
        private GameStateMachine _gameStateMachien;

        [Inject]
        private void Construct(DefeatHandler defeatHandler, IStaticDataService staticDataService, GameStateMachine gameStateMachine)
        {
            _defeatHandler = defeatHandler;
            _gameplaySettings = staticDataService.GameplaySettingsConfig;
            _gameStateMachien = gameStateMachine;

            _defeatHandler.WindowsSwitched += OnWindowsSwitched;
            _restartButton.onClick.AddListener(OnRestatrButtonClicked);
            _progressRecoveryButton.onClick.AddListener(OnProgressRecoveryButtonClicked);
            _defeatHandler.ProgressRecovery += OnProgressRecovery;
        }

        private void OnDestroy()
        {
            _defeatHandler.WindowsSwitched -= OnWindowsSwitched;
            _restartButton.onClick.RemoveListener(OnRestatrButtonClicked);
            _progressRecoveryButton.onClick.RemoveListener(OnProgressRecoveryButtonClicked);
            _defeatHandler.ProgressRecovery -= OnProgressRecovery;
        }

        private void OnProgressRecoveryButtonClicked() =>
            _defeatHandler.TryRecoveryProgress();

        private void OnRestatrButtonClicked() =>
            _gameStateMachien.Enter<GameplayLoopState>().Forget();

        private void OnProgressRecovery() =>
            Close();

        private void OnWindowsSwitched()
        {
            _progressRecoveryButton.interactable = true;

            Open();

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

            _progressRecoveryButton.interactable = false;
        }
    }
}
