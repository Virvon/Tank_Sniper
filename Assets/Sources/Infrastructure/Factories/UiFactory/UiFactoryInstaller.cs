using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Ui;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.UiFactory
{
    public class UiFactoryInstaller : Installer<UiFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IUiFactory>()
                .To<UiFactory>()
                .AsSingle();

            Container
                .BindFactory<string, UniTask<Window>, Window.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<Window>>();
        }
    }
}
