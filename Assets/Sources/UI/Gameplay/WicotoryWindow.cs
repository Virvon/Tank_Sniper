using Assets.Sources.Gameplay;
using Assets.Sources.Gameplay.Handlers;
using Assets.Sources.Infrastructure.GameStateMachine;
using Assets.Sources.Infrastructure.GameStateMachine.States;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Gameplay
{
    public class WicotoryWindow : OpenableWindow
    {
        private const string LevelInfo = "УРОВЕНЬ";

        [SerializeField] private Button _continueButton;
        [SerializeField] private TMP_Text _rewardValue;
        [SerializeField] private TMP_Text _currentLevelValue;

        private WictoryHandler _wictoryHandler;
        private GameStateMachine _gameStateMachine;
        private LoadingCurtain _loadingCurtain;
        private IStaticDataService _staticDataService;
        private IPersistentProgressService _persistentProgressService;
        private RewardCounter _rewardCounter;

        private uint _reward;

        [Inject]
        private void Construct(
            WictoryHandler wictoryHandler,
            GameStateMachine gameStateMachine,
            LoadingCurtain loadingCurtain,
            IStaticDataService staticDataService,
            IPersistentProgressService persistentProgressService,
            RewardCounter rewardCounter)
        {
            _wictoryHandler = wictoryHandler;
            _gameStateMachine = gameStateMachine;
            _loadingCurtain = loadingCurtain;
            _staticDataService = staticDataService;
            _persistentProgressService = persistentProgressService;
            _rewardCounter = rewardCounter;

            _currentLevelValue.text = $"{LevelInfo} {(_persistentProgressService.Progress.CurrentLevelIndex + 1)}";

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
            _persistentProgressService.Progress.Wallet.Give(_reward);
            SetNextLevel();
            _loadingCurtain.Show();
            _gameStateMachine.Enter<MainMenuState>().Forget();
        }

        private void SetNextLevel()
        {
            uint currentLevelIndex = _persistentProgressService.Progress.CurrentLevelIndex;
            BiomeType currentBiomeType = _persistentProgressService.Progress.CurrentBiomeType;

            if (currentLevelIndex >= _staticDataService.GetLevelsSequence(currentBiomeType).Sequence.Length - 1)
            {
                int lenght = Enum.GetValues(typeof(BiomeType)).Length;
                int nextLevelType = (int)currentBiomeType + 1;

                nextLevelType = nextLevelType >= lenght ? 0 : nextLevelType;
                _persistentProgressService.Progress.CurrentBiomeType = (BiomeType)nextLevelType;
                _persistentProgressService.Progress.CurrentLevelIndex = 0;
            }
            else
            {
                _persistentProgressService.Progress.CurrentLevelIndex++;
            }

            _persistentProgressService.Progress.CompletedLevelsCount++;
        }

        private void OnWindowsSwitched()
        {
            _reward = _rewardCounter.GetReward();
            _rewardValue.text = $"{_reward}";

            Show();
        }
    }
}
