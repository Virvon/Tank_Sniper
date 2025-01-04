using Assets.Sources.Data;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.SaveLoadProgress;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using System.Linq;

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class LoadProgressState : IState
    {
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly GameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;

        public LoadProgressState(
            IPersistentProgressService persistentProgressService,
            ISaveLoadService saveLoadService,
            GameStateMachine gameStateMachine,
            IStaticDataService staticDataService)
        {
            _persistentProgressService = persistentProgressService;
            _saveLoadService = saveLoadService;
            _gameStateMachine = gameStateMachine;
            _staticDataService = staticDataService;
        }

        public UniTask Enter()
        {
            LoadProgressOrInitNew();
            _gameStateMachine.Enter<MainMenuState>().Forget();

            return default;
        }

        public UniTask Exit() =>
            default;

        private void LoadProgressOrInitNew() =>
            _persistentProgressService.Progress = _saveLoadService.LoadProgress() ?? CreateNewProgress();

        private PlayerProgress CreateNewProgress()
        {
            DecalData[] decalDatas = _staticDataService.DecalConfigs.Select(config => new DecalData(config.Type, config.IsUnlockedOnStart)).ToArray();
            DecalType startDecal = decalDatas.First(decal => decal.IsUnlocked).Type;
            TankData[] tankDatas = _staticDataService.TankConfigs.Select(config => new TankData(config.Level, config.IsUnlockOnStart, startDecal)).ToArray();
            TankSkinData[] tankSkinDatas = _staticDataService.TankSkinConfigs.Select(config => new TankSkinData(config.Type)).ToArray();

            PlayerProgress progress = new(tankDatas, tankSkinDatas, decalDatas, 0);

            return progress;
        }
    }
}
