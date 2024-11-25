using Assets.Sources.Services.SceneManagment;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class GameplayLoopState : IState
    {
        private readonly ISceneLoader _sceneLoader;

        public GameplayLoopState(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public async UniTask Enter() =>
            await _sceneLoader.Load(InfrastructureAssetPath.GameplayScene);

        public UniTask Exit() =>
            default;
    }
}
