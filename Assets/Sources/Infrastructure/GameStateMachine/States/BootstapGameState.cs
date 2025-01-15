using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService;
using Cysharp.Threading.Tasks;
using System.Collections;
using System;
using Assets.Sources.Services.CoroutineRunner;
using Agava.YandexGames;

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class BootstapGameState : IState
    {
        private readonly IAssetProvider _assetProvider;
        private readonly GameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly ICoroutineRunner _coroutineRunner;

        public BootstapGameState(
            IAssetProvider assetProvider,
            GameStateMachine gameStateMachine,
            IStaticDataService staticDataService,
            ICoroutineRunner coroutineRunner)
        {
            _assetProvider = assetProvider;
            _gameStateMachine = gameStateMachine;
            _staticDataService = staticDataService;
            _coroutineRunner = coroutineRunner;
        }

        public async UniTask Enter()
        {
            await Initialize();

            _coroutineRunner.StartCoroutine(InitializeYandexSdk(callback: () => _gameStateMachine.Enter<LoadProgressState>().Forget()));
        }

        public UniTask Exit() =>
            default;

        private async UniTask Initialize()
        {
            await _assetProvider.InitializeAsync();
            await _staticDataService.InitializeAsync();
        }

        private IEnumerator InitializeYandexSdk(Action callback)
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            callback?.Invoke();
            yield break;
#else
            yield return YandexGamesSdk.Initialize();

            if (YandexGamesSdk.IsInitialized == false)
                throw new ArgumentNullException(nameof(YandexGamesSdk), "Yandex SDK didn't initialized correctly");

            YandexGamesSdk.CallbackLogging = true;
            StickyAd.Show();
            callback?.Invoke();
#endif
        }
    }
}
