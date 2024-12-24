using Assets.Sources.Gameplay.Player;
using Assets.Sources.MainMenu;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Tanks;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.TankFactory
{
    public class TankFactory : ITankFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly DiContainer _container;
        private readonly Tank.Factory _tankFactory;
        private readonly TankShootingWrapper.Factory _tankShootingWrapperFactory;
        private readonly PlayerTankWrapper.Factory _playerTankWrapperFactory;

        public TankFactory(
            IAssetProvider assetProvider,
            IStaticDataService staticDataService,
            DiContainer container,
            Tank.Factory tankFactory,
            TankShootingWrapper.Factory tankShootingWrapperFactory,
            PlayerTankWrapper.Factory playerTankWrapperFactory)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _container = container;
            _tankFactory = tankFactory;
            _tankShootingWrapperFactory = tankShootingWrapperFactory;
            _playerTankWrapperFactory = playerTankWrapperFactory;
        }

        public async UniTask<PlayerTankWrapper> CreatePlayerTankWrapper(uint tankLevel, Vector3 position, Quaternion rotation)
        {
            PlayerTankWrapper wrapper = await _playerTankWrapperFactory.Create(_staticDataService.GetTank(tankLevel).GameplayWrapperAssetReference, position, rotation);

            _container.BindInstance(wrapper).AsSingle();

            return wrapper;
        }

        public async UniTask<Tank> CreateTank(
            uint level,
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            TankSkinType skinType = TankSkinType.Base,
            DecalType decalType = DecalType.Decal1,
            bool isDecalsChangable = false)
        {
            Tank tank = await _tankFactory.Create(_staticDataService.GetTank(level).AssetReference, position, rotation, parent);
            Material skinMaterial;

            if (skinType == TankSkinType.Base)
                skinMaterial = await _assetProvider.Load<Material>(_staticDataService.GetTank(level).BaseMaterialAssetReference);
            else
                skinMaterial = await _assetProvider.Load<Material>(_staticDataService.GetSkin(skinType).MaterialAssetReference);

            tank.Initialize(level, skinMaterial, decalType, isDecalsChangable);

            return tank;
        }

        public async UniTask<TankShootingWrapper> CreateTankShootingWrapper(uint tankLevel, Vector3 position, Quaternion rotation, Transform parent) =>
            await _tankShootingWrapperFactory.Create(_staticDataService.GetTank(tankLevel).MainMenuWrapperAssetReference, position, rotation, parent);
    }
}
