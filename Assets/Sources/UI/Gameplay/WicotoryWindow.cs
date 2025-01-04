using Assets.Sources.Gameplay.Handlers;
using Assets.Sources.Infrastructure.GameStateMachine;
using Assets.Sources.Infrastructure.GameStateMachine.States;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Gameplay
{
    public class WicotoryWindow : OpenableWindow
    {
        [SerializeField] private Button _continueButton;

        private WictoryHandler _wictoryHandler;
        private GameStateMachine _gameStateMachine;
        private LoadingCurtain _loadingCurtain;

        [Inject]
        private void Construct(WictoryHandler wictoryHandler, GameStateMachine gameStateMachine, LoadingCurtain loadingCurtain)
        {
            _wictoryHandler = wictoryHandler;
            _gameStateMachine = gameStateMachine;
            _loadingCurtain = loadingCurtain;

            _wictoryHandler.WindowsSwithed += OnWindowsSwitched;
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        private void OnDestroy()
        {
            _wictoryHandler.WindowsSwithed -= OnWindowsSwitched;
            _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        }

        private void OnContinueButtonClicked()
        {
            _loadingCurtain.Show();
            _gameStateMachine.Enter<MainMenuState>().Forget();
        }

        private void OnWindowsSwitched() =>
            Show();
    }
}
