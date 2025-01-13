using Assets.Sources.MainMenu;
using Assets.Sources.MainMenu.Desk;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Tanks;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.MainMenuFactory
{
    public class MainMenuFactory : IMainMenuFactory
    {
        private readonly DiContainer _container;
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetProvider _assetProvider;
        private readonly Tank.Factory _tankFactory;
        private readonly Desk.Factory _deskFactory;
        private readonly TankShootingWrapper.Factory _tankShootingWrapper;

        public MainMenuFactory(
            DiContainer container,
            IStaticDataService staticDataService,
            IAssetProvider assetProvider,
            Tank.Factory tankFactory,
            Desk.Factory deskFactory,
            TankShootingWrapper.Factory tankShootingWrapper)
        {
            _container = container;
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
            _tankFactory = tankFactory;
            _deskFactory = deskFactory;
            _tankShootingWrapper = tankShootingWrapper;
        }

        public async UniTask CreateDesk()
        {
            Desk desk = await _deskFactory.Create(MainMenuFactoryAssets.Desk);

            _container.BindInstance(desk).AsSingle();
        }
    }
}
