using Assets.Sources.Gameplay;
using Assets.Sources.Gameplay.Bullets;
using Assets.Sources.Gameplay.Enemies;
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
                .BindFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<ExplosionBullet>, ExplosionBullet.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<ExplosionBullet>>();

            Container
                .BindFactory<string, UniTask<PlayerTank>, PlayerTank.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<PlayerTank>>();

            Container
                .BindFactory<string, UniTask<GameplayCamera>, GameplayCamera.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<GameplayCamera>>();

            Container
                .BindFactory<string, Vector3, Quaternion, UniTask<Enemy>, Enemy.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<Enemy>>();
        }
    }
}
