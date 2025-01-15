using Assets.Sources.Infrastructure.GameStateMachine;
using Assets.Sources.Infrastructure.GameStateMachine.States;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Gameplay
{
    public class RestartWindow : OpenableWindow
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button[] _hideWindowButtons;

        private GameStateMachine _gameStateMachine;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;

            _restartButton.onClick.AddListener(OnRestartButtonClicked);

            foreach (Button button in _hideWindowButtons)
                button.onClick.AddListener(Hide);
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);

            foreach (Button button in _hideWindowButtons)
                button.onClick.RemoveListener(Hide);
        }

        public override void Show()
        {
            base.Show();
            Time.timeScale = 0;
        }

        public override void Hide()
        {
            base.Hide();
            Time.timeScale = 1;
        }

        private void OnRestartButtonClicked()
        {
            Time.timeScale = 1;
            _gameStateMachine.Enter<GameplayLoopState>().Forget();
        }
    }
}
