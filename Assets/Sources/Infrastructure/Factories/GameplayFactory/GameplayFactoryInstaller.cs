using Assets.Sources.Gameplay;
using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Gameplay.Weapons;
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
                .BindFactory<string, UniTask<PlayerTank>, PlayerTank.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<PlayerTank>>();

            Container
                .BindFactory<string, UniTask<GameplayCamera>, GameplayCamera.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<GameplayCamera>>();

            Container
                .BindFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<Enemy>, Enemy.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<Enemy>>();

            Container
                .BindFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<TankRocket>, TankRocket.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<TankRocket>>();
            
            Container
                .BindFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<Laser>, Laser.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<Laser>>();
            
            Container
                .BindFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<HomingBullet>, HomingBullet.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<HomingBullet>>();
            
            Container
                .BindFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<TransmittingLaser>, TransmittingLaser.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<TransmittingLaser>>();
            
            Container
                .BindFactory<AssetReferenceGameObject, Vector3, UniTask<Laser2>, Laser2.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<Laser2>>();
            
            Container
                .BindFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<CompositeBullet>, CompositeBullet.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<CompositeBullet>>();
            
            Container
                .BindFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<Bomb>, Bomb.Factory>()
                .FromFactory<ReferencePrefabFactoryAsync<Bomb>>();
        }
    }
}
