using Assets.Sources.Gameplay.Handlers;
using Assets.Sources.Infrastructure.GameStateMachine;
using Assets.Sources.Infrastructure.GameStateMachine.States;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Gameplay.DefeatWindows
{
    public class DefeatWindow : OpenableWindow
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _progressRecoveryButton;

        private DefeatHandler _defeatHandler;
        private GameStateMachine _gameStateMachien;

        public Button ProgressRecoveryButton => _progressRecoveryButton;

        [Inject]
        private void Construct(DefeatHandler defeatHandler, GameStateMachine gameStateMachine)
        {
            _defeatHandler = defeatHandler;
            _gameStateMachien = gameStateMachine;

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
