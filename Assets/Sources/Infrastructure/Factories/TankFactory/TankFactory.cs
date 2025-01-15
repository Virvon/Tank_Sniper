using Assets.Sources.Gameplay.Player;
using Assets.Sources.Gameplay.Player.Weapons;
using Assets.Sources.Gameplay.Player.Wrappers;
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
        private readonly PlayerDroneWrapper.Factory _playerDronFactory;
        private readonly Drone.Factory _droneFactory;
        private readonly PlayerCharacter.Factory _playerCharacterFactory;

        public TankFactory(
            IAssetProvider assetProvider,
            IStaticDataService staticDataService,
            DiContainer container,
            Tank.Factory tankFactory,
            TankShootingWrapper.Factory tankShootingWrapperFactory,
            PlayerTankWrapper.Factory playerTankWrapperFactory,
            PlayerDroneWrapper.Factory playerDronFactory,
            Drone.Factory droneFactory,
            PlayerCharacter.Factory playerCharacterFactory)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _container = container;
            _tankFactory = tankFactory;
            _tankShootingWrapperFactory = tankShootingWrapperFactory;
            _playerTankWrapperFactory = playerTankWrapperFactory;
            _playerDronFactory = playerDronFactory;
            _droneFactory = droneFactory;
            _playerCharacterFactory = playerCharacterFactory;
        }

        public async UniTask<PlayerCharacter> CreatePlayerCharacter(PlayerCharacterType type, Vector3 position, Quaternion rotation, Transform parent) =>
            await _playerCharacterFactory.Create(_staticDataService.GetPlayerCharacter(type).AssetReference, position, rotation, parent);

        public async UniTask<Drone> CreateDrone(Vector3 position, Quaternion rotation) =>
            await _droneFactory.Create(TankFactoryAssets.Drone, position, rotation);

        public async UniTask CreatePlayerDroneWrapper(Vector3 position, Quaternion rotation)
        {
            PlayerWrapper wrapper = await _playerDronFactory.Create(TankFactoryAssets.PlayerDroneWrapper, position, rotation);
            _container.BindInstance(wrapper).AsSingle();
            _container.BindInterfacesAndSelfTo<PlayerDroneWrapper>().FromInstance((PlayerDroneWrapper)wrapper).AsSingle();
        }

        public async UniTask<PlayerTankWrapper> CreatePlayerTankWrapper(uint tankLevel, Vector3 position, Quaternion rotation)
        {
            PlayerTankWrapper wrapper = await _playerTankWrapperFactory.Create(_staticDataService.GetTank(tankLevel).GameplayWrapperAssetReference, position, rotation);

            _container.BindInstance(wrapper).AsSingle();
            _container.BindInstance(wrapper as PlayerWrapper).AsSingle();
            _container.BindInterfacesAndSelfTo<PlayerTankWeapon>().FromInstance(wrapper.GetComponent<PlayerTankWeapon>()).AsSingle();

            return wrapper;
        }

        public async UniTask<Tank> CreateTank(
            uint level,
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            string skinId,
            DecalType decalType = DecalType.Decal1,
            bool isDecalsChangable = false)
        {
            Material skinMaterial;

            if (skinId == string.Empty)
                skinMaterial = await _assetProvider.Load<Material>(_staticDataService.GetTank(level).BaseMaterialAssetReference);
            else
                skinMaterial = await _assetProvider.Load<Material>(_staticDataService.GetSkin(skinId).MaterialAssetReference);

            Tank tank = await _tankFactory.Create(_staticDataService.GetTank(level).AssetReference, position, rotation, parent);

            tank.Initialize(level, skinMaterial, decalType, isDecalsChangable);

            return tank;
        }

        public async UniTask<TankShootingWrapper> CreateTankShootingWrapper(uint tankLevel, Vector3 position, Quaternion rotation, Transform parent) =>
            await _tankShootingWrapperFactory.Create(_staticDataService.GetTank(tankLevel).MainMenuWrapperAssetReference, position, rotation, parent);
    }
}
