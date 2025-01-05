using Assets.Sources.Services.InputService;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.SceneManagment;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Types;
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

        private uint _currentLevelIndex;
        private BiomeType _currentLevelType;

        public GameplayLoopState(
            ISceneLoader sceneLoader,
            IInputService inputService,
            IStaticDataService staticDataService,
            IPersistentProgressService persistentProgressService)
        {
            _sceneLoader = sceneLoader;
            _inputService = inputService;
            _staticDataService = staticDataService;
            _persistentProgressService = persistentProgressService;
        }

        public async UniTask Enter()
        {
            _inputService.SetActive(true);
            _currentLevelIndex = _persistentProgressService.Progress.CurrentLevelIndex;
            _currentLevelType = _persistentProgressService.Progress.CurrentLevelType;
            await _sceneLoader.Load(_staticDataService.GetLevelsSequence(_currentLevelType).GetLevel(_currentLevelIndex));
        }

        public UniTask Exit()
        {
            if(_currentLevelIndex >= _staticDataService.GetLevelsSequence(_currentLevelType).Sequence.Length - 1)
            {
                int lenght = Enum.GetValues(typeof(BiomeType)).Length;
                int nextLevelType = (int)_currentLevelType + 1;

                nextLevelType = nextLevelType >= lenght ? 0 : nextLevelType;
                _persistentProgressService.Progress.CurrentLevelType = (BiomeType)nextLevelType;
                _persistentProgressService.Progress.CurrentLevelIndex = 0;
            }
            else
            {
                _persistentProgressService.Progress.CurrentLevelIndex++;
            }

            return default;
        }
    }
}
