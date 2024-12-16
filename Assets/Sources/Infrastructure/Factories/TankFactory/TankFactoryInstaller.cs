using Assets.Sources.Gameplay;
using Assets.Sources.MainMenu;
using Assets.Sources.Services.AssetManagement;
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
        }
    }
}
