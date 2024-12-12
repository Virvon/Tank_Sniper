﻿using Assets.Sources.MainMenu;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StaticDataService;
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

        public MainMenuFactory(
            DiContainer container,
            IStaticDataService staticDataService,
            IAssetProvider assetProvider,
            Tank.Factory tankFactory,
            Desk.Factory deskFactory
            )
        {
            _container = container;
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
            _tankFactory = tankFactory;
            _deskFactory = deskFactory;
        }

        public async UniTask<Tank> CreateTank(uint level, Vector3 position, Quaternion rotation, Transform parent, TankSkinType skinType = TankSkinType.Base)
        {
            Tank tank = await _tankFactory.Create(_staticDataService.GetTank(level).AssetReference, position, rotation, parent);
            Material skinMaterial;

            if(skinType == TankSkinType.Base)
                skinMaterial = await _assetProvider.Load<Material>(_staticDataService.GetTank(level).BaseMaterial);
            else
                skinMaterial = await _assetProvider.Load<Material>(_staticDataService.GetSkin(skinType).Material);

            tank.Initialize(level, skinMaterial);

            return tank;
        }

        public async UniTask CreateDesk()
        {
            Desk desk = await _deskFactory.Create(MainMenuFactoryAssets.Desk);

            _container.BindInstance(desk).AsSingle();
        }
    }
}
