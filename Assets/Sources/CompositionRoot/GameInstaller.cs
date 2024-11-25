using Assets.Sources.Infrastructure.GameStateMachine;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.CoroutineRunner;
using Assets.Sources.Services.InputService;
using Assets.Sources.Services.SceneManagment;
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
        }

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
