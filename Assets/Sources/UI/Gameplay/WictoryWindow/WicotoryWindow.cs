using Assets.Sources.Gameplay;
using Assets.Sources.Gameplay.Handlers;
using Assets.Sources.Infrastructure.GameStateMachine;
using Assets.Sources.Infrastructure.GameStateMachine.States;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.SaveLoadProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Gameplay.WictoryWindow
{
    public class WicotoryWindow : OpenableWindow
    {
        private const string LevelInfo = "УРОВЕНЬ";
        private const float CloseDelay = 0.5f;

        [SerializeField] private Button _continueButton;
        [SerializeField] private TMP_Text _rewardValue;
        [SerializeField] private TMP_Text _currentLevelValue;
        [SerializeField] private WalletPanel _walletPanel;
        [SerializeField] private MoneyEffect _moneyEffect;

        private WictoryHandler _wictoryHandler;
        private GameStateMachine _gameStateMachine;
        private GameplayLoadingCurtain _loadingCurtain;
        private IStaticDataService _staticDataService;
        private IPersistentProgressService _persistentProgressService;
        private RewardCounter _rewardCounter;
        private ISaveLoadService _saveLoadService;

        private uint _reward;

        protected RewardCounter RewardCounter => _rewardCounter;
        protected IPersistentProgressService PersistentProgressService => _persistentProgressService;

        [Inject]
        private void Construct(
            WictoryHandler wictoryHandler,
            GameStateMachine gameStateMachine,
            GameplayLoadingCurtain loadingCurtain,
            IStaticDataService staticDataService,
            IPersistentProgressService persistentProgressService,
            RewardCounter rewardCounter,
            ISaveLoadService saveLoadService)
        {
            _wictoryHandler = wictoryHandler;
            _gameStateMachine = gameStateMachine;
            _loadingCurtain = loadingCurtain;
            _staticDataService = staticDataService;
            _persistentProgressService = persistentProgressService;
            _rewardCounter = rewardCounter;
            _saveLoadService = saveLoadService;

            _currentLevelValue.text = $"{LevelInfo} {_persistentProgressService.Progress.CurrentLevelIndex + 1}";

            _wictoryHandler.WindowsSwithed += OnWindowsSwitched;
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        protected virtual void OnDestroy()
        {
            _wictoryHandler.WindowsSwithed -= OnWindowsSwitched;
            _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        }

        protected virtual void OnWindowsSwitched()
        {
            _reward = _rewardCounter.GetReward();
            _rewardValue.text = $"{_reward}";

            Show();
        }

        protected void LoadNextScene()
        {
            _loadingCurtain.Show();
            _gameStateMachine.Enter<MainMenuState>().Forget();
            SetNextLevel();
        }

        private void OnContinueButtonClicked()
        {
            StartCoroutine(Animator(_staticDataService.AnimationsConfig.WalletValueChangingDuration, callback: LoadNextScene));

            _walletPanel.CreditReward(_reward, _staticDataService.AnimationsConfig.WalletValueChangingDuration);
            _moneyEffect.Play();

            _persistentProgressService.Progress.Wallet.Give(_reward);
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
            _saveLoadService.SaveProgress();
        }

        private IEnumerator Animator(float duration, Action callback)
        {
            float progress;
            float passedTime = 0;
            uint startValue = _reward;
            bool isAnimated = true;

            while (isAnimated)
            {
                progress = passedTime / duration;
                passedTime += Time.deltaTime;

                int value = (int)Mathf.Lerp(startValue, 0, progress);
                _rewardValue.text = value.ToString();

                if(value == 0)
                    isAnimated = false;

                yield return null;
            }

            yield return new WaitForSeconds(CloseDelay);

            callback?.Invoke();
        }
    }
}
