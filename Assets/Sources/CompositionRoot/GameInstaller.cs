using Assets.Sources.Infrastructure.GameStateMachine;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.CoroutineRunner;
using Assets.Sources.Services.InputService;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.SaveLoadProgress;
using Assets.Sources.Services.SceneManagment;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.UI.Gameplay;
using Assets.Sources.UI.LoadingCurtain;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets.Sources.CompositionRoot
{
    public class GameInstaller : MonoInstaller, ICoroutineRunner
    {
        public override void InstallBindings()
        {
            BindInputService();
            BindGameStateMachine();
            BindSceneLoader();
            BindAssetProvider();
            BindCoroutineRunner();
            BindStaticDataService();
            BindPersistentProgressService();
            BindSaveLoadService();
            BindLoadingCurtain();
        }

        private void BindLoadingCurtain()
        {
            Container
                .BindFactory<string, UniTask<LoadingCurtain>, LoadingCurtain.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<LoadingCurtain>>();

            Container
                .BindInterfacesAndSelfTo<LoadingCurtainProxy>()
                .AsSingle();
        }

        private void BindSaveLoadService() =>
            Container.BindInterfacesTo<SaveLoadService>().AsSingle();

        private void BindPersistentProgressService() =>
            Container.BindInterfacesTo<PersistentProgressService>().AsSingle();

        private void BindStaticDataService() =>
            Container.BindInterfacesTo<StaticDataService>().AsSingle();

        private void BindCoroutineRunner() =>
            Container.Bind(typeof(ICoroutineRunner)).FromInstance(this).AsSingle();

        private void BindInputService() =>
            Container.BindInterfacesTo<InputService>().AsSingle();

        private void BindGameStateMachine() =>
            GameStateMachineInstaller.Install(Container);

        private void BindSceneLoader() =>
            Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle();

        private void BindAssetProvider() =>
            Container.BindInterfacesAndSelfTo<AssetProvider>().AsSingle();
    }
}
