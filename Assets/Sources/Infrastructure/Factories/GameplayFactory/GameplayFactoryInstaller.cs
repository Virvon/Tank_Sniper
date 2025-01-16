using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Gameplay.Player;
using Assets.Sources.Services.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.GameplayFactory
{
    public class GameplayFactoryInstaller : Installer<GameplayFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Container
               .Bind<IGameplayFactory>()
               .To<GameplayFactory>()
               .AsSingle();

            Container
                .BindFactory<string, UniTask<GameplayCamera>, GameplayCamera.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<GameplayCamera>>();

            Container
                .BindFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<Enemy>, Enemy.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<Enemy>>();
            
            Container
                .BindFactory<string, Vector3, Quaternion, UniTask<RotationCamera>, RotationCamera.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<RotationCamera>>();
            
            Container
                .BindFactory<string, Transform, UniTask<CameraNoise>, CameraNoise.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<CameraNoise>>();
        }
    }
}
