using Assets.Sources.Services.InputService;
using Assets.Sources.Services.SceneManagment;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class GameplayLoopState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IInputService _inputService;

        public GameplayLoopState(ISceneLoader sceneLoader, IInputService inputService)
        {
            _sceneLoader = sceneLoader;
            _inputService = inputService;
        }

        public async UniTask Enter()
        {
            _inputService.SetActive(true);
            await _sceneLoader.Load(InfrastructureAssetPath.GameplayScene);
        }

        public UniTask Exit() =>
            default;
    }
}
