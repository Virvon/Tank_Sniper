using Assets.Sources.Services.InputService;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.SceneManagment;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Types;
using Assets.Sources.UI.LoadingCurtain;
using Cysharp.Threading.Tasks;
using System;

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class GameplayLoopState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IInputService _inputService;
        private readonly IStaticDataService _staticDataService;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ILoadingCurtain _loadingCurtain;

        private uint _currentLevelIndex;
        private BiomeType _currentLevelType;

        public GameplayLoopState(
            ISceneLoader sceneLoader,
            IInputService inputService,
            IStaticDataService staticDataService,
            IPersistentProgressService persistentProgressService,
            ILoadingCurtain loadingCurtain)
        {
            _sceneLoader = sceneLoader;
            _inputService = inputService;
            _staticDataService = staticDataService;
            _persistentProgressService = persistentProgressService;
            _loadingCurtain = loadingCurtain;
        }

        public async UniTask Enter()
        {
            _loadingCurtain.Hide();
            _inputService.SetActive(true);
            _currentLevelIndex = _persistentProgressService.Progress.CurrentLevelIndex;
            _currentLevelType = _persistentProgressService.Progress.CurrentBiomeType;
            await _sceneLoader.Load(_staticDataService.GetLevelsSequence(_currentLevelType).GetLevel(_currentLevelIndex));
        }

        public UniTask Exit()
        {
            _loadingCurtain.Show();
            return default;
        }
    }
}
