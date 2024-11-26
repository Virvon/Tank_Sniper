using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class BootstapGameState : IState
    {
        private readonly IAssetProvider _assetProvider;
        private readonly GameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;

        public BootstapGameState(IAssetProvider assetProvider, GameStateMachine gameStateMachine, IStaticDataService staticDataService)
        {
            _assetProvider = assetProvider;
            _gameStateMachine = gameStateMachine;
            _staticDataService = staticDataService;
        }

        public async UniTask Enter()
        {
            await Initialize();

            _gameStateMachine.Enter<MainMenuState>().Forget();
        }

        public UniTask Exit() =>
            default;

        private async UniTask Initialize()
        {
            await _assetProvider.InitializeAsync();
            await _staticDataService.InitializeAsync();
        }
    }
}
