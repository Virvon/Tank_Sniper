using Assets.Sources.MainMenu;
using Assets.Sources.Services.StaticDataService;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.MainMenuFactory
{
    public class MainMenuFactory : IMainMenuFactory
    {
        private readonly DiContainer _container;
        private readonly IStaticDataService _staticDataService;
        private readonly Tank.Factory _tankFactory;
        private readonly Desk.Factory _deskFactory;

        public MainMenuFactory(DiContainer container, IStaticDataService staticDataService, Tank.Factory tankFactory, Desk.Factory deskFactory)
        {
            _container = container;
            _staticDataService = staticDataService;
            _tankFactory = tankFactory;
            _deskFactory = deskFactory;
        }

        public async UniTask<Tank> CreateTank(uint tankLevel, Quaternion rotation)
        {
            Tank tank = await _tankFactory.Create(_staticDataService.GetTank(tankLevel).AssetReference, rotation);

            tank.Initialize(tankLevel);

            return tank;
        }

        public async UniTask CreateDesk()
        {
            Desk desk = await _deskFactory.Create(MainMenuFactoryAssets.Desk);

            _container.BindInstance(desk).AsSingle();
        }
    }
}
