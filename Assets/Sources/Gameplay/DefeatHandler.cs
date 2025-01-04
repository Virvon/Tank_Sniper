using Assets.Sources.Services.CoroutineRunner;
using Assets.Sources.Services.InputService;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay
{
    public class DefeatHandler
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly GameplaySettingsConfig _gameplaySettings;
        private readonly IInputService _inputService;

        public event Action Defeated;
        public event Action WindowsSwitched;
        public event Action ProgressRecovery;

        public DefeatHandler(
            ICoroutineRunner coroutineRunner,
            IStaticDataService staticDataService,
            IInputService inputService)
        {
            _coroutineRunner = coroutineRunner;
            _gameplaySettings = staticDataService.GameplaySettingsConfig;
            _inputService = inputService;
        }

        public void TryRecoveryProgress()
        {
            _inputService.SetActive(true);
            ProgressRecovery?.Invoke();
        }

        public void OnPlayerDestructed()
        {
            Defeated?.Invoke();
            _inputService.SetActive(false);
            _coroutineRunner.StartCoroutine(Timer(callback: () => WindowsSwitched?.Invoke()));
        }

        private IEnumerator Timer(Action callback)
        {
            yield return new WaitForSeconds(_gameplaySettings.DefeatWaitingDelay);

            callback?.Invoke();
        }
    }
}
