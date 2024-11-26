using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class BootstapGameState : IState
    {
        private readonly IAssetProvider _assetProvider;
        private readonly GameStateMachine _gameStateMachine;

        public BootstapGameState(IAssetProvider assetProvider, GameStateMachine gameStateMachine)
        {
            _assetProvider = assetProvider;
            _gameStateMachine = gameStateMachine;
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
        }
    }
}
