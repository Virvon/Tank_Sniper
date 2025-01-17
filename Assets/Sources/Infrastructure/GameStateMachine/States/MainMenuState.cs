using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.InputService;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.SaveLoadProgress;
using Assets.Sources.Services.SceneManagment;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.UI.LoadingCurtain;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class MainMenuState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetProvider _assetProvider;
        private readonly IInputService _inputService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly ILoadingCurtain _loadingCurtain;

        public MainMenuState(
            ISceneLoader sceneLoader,
            IPersistentProgressService persistentProgressService,
            IStaticDataService staticDataService,
            IAssetProvider assetProvider,
            IInputService inputService,
            ISaveLoadService saveLoadService,
            ILoadingCurtain loadingCurtain)
        {
            _sceneLoader = sceneLoader;
            _persistentProgressService = persistentProgressService;
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
            _inputService = inputService;
            _saveLoadService = saveLoadService;
            _loadingCurtain = loadingCurtain;
        }

        public async UniTask Enter()
        {
            _loadingCurtain.Hide();
            await _assetProvider.WarmupAssetsByLable(AssetLabels.MainMenu);
            await _sceneLoader.Load(_staticDataService.GetLevelsSequence(_persistentProgressService.Progress.CurrentBiomeType).MainMenuScene);
            _inputService.SetActive(true);
        }

        public async UniTask Exit()
        {
            _loadingCurtain.Show();
            _saveLoadService.SaveProgress();
            await _assetProvider.ReleaseAssetsByLabel(AssetLabels.MainMenu);
        }
    }
}
