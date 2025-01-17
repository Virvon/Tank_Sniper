using Assets.Sources.Gameplay.Player;
using Assets.Sources.Gameplay.Player.Wrappers;
using Assets.Sources.MainMenu;
using Assets.Sources.MainMenu.Desk;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Tanks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.TankFactory
{
    public class TankFactoryInstaller : Installer<TankFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<TankFactory>().AsSingle();

            Container
                .BindFactory<AssetReferenceGameObject, Vector3, Quaternion, Transform, UniTask<TankShootingWrapper>, TankShootingWrapper.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<TankShootingWrapper>>();

            Container
                .BindFactory<AssetReferenceGameObject, Vector3, Quaternion, Transform, UniTask<Tank>, Tank.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<Tank>>();
            
            Container
                .BindFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<PlayerTankWrapper>, PlayerTankWrapper.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<PlayerTankWrapper>>();
            
            Container
                .BindFactory<AssetReferenceGameObject, Vector3, Quaternion, Transform, UniTask<PlayerCharacter>, PlayerCharacter.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<PlayerCharacter>>();
            
            Container
                .BindFactory<string, Vector3, Quaternion, UniTask<PlayerWrapper>, PlayerDroneWrapper.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<PlayerWrapper>>();
            
            Container
                .BindFactory<string, Vector3, Quaternion, UniTask<Drone>, Drone.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<Drone>>();
            
            Container
                .BindFactory<string, Vector3, Quaternion, Transform, UniTask<PlayerAccessor>, PlayerAccessor.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<PlayerAccessor>>();
            
            Container
                .BindFactory<string, Vector3, Transform, UniTask<DeskTankWrapper>, DeskTankWrapper.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<DeskTankWrapper>>();
        }
    }
}
