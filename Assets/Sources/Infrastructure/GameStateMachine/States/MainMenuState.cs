using Assets.Sources.Services.SceneManagment;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class MainMenuState : IState
    {
        private readonly ISceneLoader _sceneLoader;

        public MainMenuState(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public async UniTask Enter() =>
            await _sceneLoader.Load(InfrastructureAssetPath.MainMenuScene);

        public UniTask Exit() =>
            default;
    }
}
