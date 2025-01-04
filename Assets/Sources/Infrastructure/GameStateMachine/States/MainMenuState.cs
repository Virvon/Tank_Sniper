using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.SceneManagment;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class MainMenuState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IStaticDataService _staticDataService;

        public MainMenuState(ISceneLoader sceneLoader, IPersistentProgressService persistentProgressService, IStaticDataService staticDataService)
        {
            _sceneLoader = sceneLoader;
            _persistentProgressService = persistentProgressService;
            _staticDataService = staticDataService;
        }

        public async UniTask Enter() =>
            await _sceneLoader.Load(_staticDataService.GetLevelsSequence(_persistentProgressService.Progress.CurrentLevelType).MainMenuScene);

        public UniTask Exit() =>
            default;
    }
}
