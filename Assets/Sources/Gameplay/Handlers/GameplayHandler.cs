using Assets.Sources.Services.CoroutineRunner;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Handlers
{
    public class GameplayHandler
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly GameplaySettingsConfig _gameplaySettings;

        public GameplayHandler(ICoroutineRunner coroutineRunner, IStaticDataService staticDataService)
        {
            _coroutineRunner = coroutineRunner;
            _gameplaySettings = staticDataService.GameplaySettingsConfig;
        }

        protected void StartTimer(Action callback) =>
            _coroutineRunner.StartCoroutine(Timer(callback));

        private IEnumerator Timer(Action callback)
        {
            yield return new WaitForSeconds(_gameplaySettings.WindowsSwitchDeley);

            callback?.Invoke();
        }
    }
}
