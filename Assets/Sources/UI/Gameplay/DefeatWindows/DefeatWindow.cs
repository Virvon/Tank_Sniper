using Assets.Sources.Gameplay;
using Assets.Sources.Gameplay.Handlers;
using Assets.Sources.Infrastructure.GameStateMachine;
using Assets.Sources.Infrastructure.GameStateMachine.States;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Gameplay.DefeatWindows
{
    public class DefeatWindow : OpenableWindow
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _progressRecoveryButton;
        [SerializeField] private TMP_Text _rewardValue;

        private DefeatHandler _defeatHandler;
        private GameStateMachine _gameStateMachien;
        private RewardCounter _rewardCounter;

        public Button ProgressRecoveryButton => _progressRecoveryButton;

        [Inject]
        private void Construct(DefeatHandler defeatHandler, GameStateMachine gameStateMachine, RewardCounter rewardCounter)
        {
            _defeatHandler = defeatHandler;
            _gameStateMachien = gameStateMachine;
            _rewardCounter = rewardCounter;

            _defeatHandler.WindowsSwitched += OnWindowsSwitched;
            _restartButton.onClick.AddListener(OnRestatrButtonClicked);
            _progressRecoveryButton.onClick.AddListener(OnProgressRecoveryButtonClicked);
            _defeatHandler.ProgressRecovered += OnProgressRecovery;
        }

        private void OnDestroy()
        {
            _defeatHandler.WindowsSwitched -= OnWindowsSwitched;
            _restartButton.onClick.RemoveListener(OnRestatrButtonClicked);
            _progressRecoveryButton.onClick.RemoveListener(OnProgressRecoveryButtonClicked);
            _defeatHandler.ProgressRecovered -= OnProgressRecovery;
        }

        protected virtual void OnWindowsSwitched()
        {
            _progressRecoveryButton.interactable = true;

            _rewardValue.text = $"{_rewardCounter.GetReward()}";

            Show();
        }

        protected virtual void OnProgressRecovery() =>
            Hide();

        private void OnProgressRecoveryButtonClicked() =>
            _defeatHandler.TryRecoveryProgress();

        private void OnRestatrButtonClicked() =>
            _gameStateMachien.Enter<GameplayLoopState>().Forget();
    }
}
