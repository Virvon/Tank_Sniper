using Assets.Sources.MainMenu;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.MainMenuFactory
{
    public class MainMenuFactory : IMainMenuFactory
    {
        private readonly Tank.Factory _tankFactory;
        private readonly Desk.Factory _deskFactory;

        public MainMenuFactory(Tank.Factory tankFactory, Desk.Factory deskFactory)
        {
            _tankFactory = tankFactory;
            _deskFactory = deskFactory;
        }

        public async UniTask<Tank> CreateTank(Vector3 position, Quaternion rotation, Transform parent) =>
            await _tankFactory.Create(MainMenuFactoryAssets.Tank, position, rotation, parent);

        public async UniTask CreateDesk() =>
            await _deskFactory.Create(MainMenuFactoryAssets.Desk);
    }
}
