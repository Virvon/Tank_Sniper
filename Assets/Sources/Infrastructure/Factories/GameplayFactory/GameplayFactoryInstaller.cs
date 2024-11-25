using Assets.Sources.Gameplay;
using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Services.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
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
                .BindFactory<string, Vector3, Quaternion, UniTask<Bullet>, Bullet.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<Bullet>>();

            Container
                .BindFactory<string, UniTask<PlayerTank>, PlayerTank.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<PlayerTank>>();

            Container
                .BindFactory<string, UniTask<GameplayCamera>, GameplayCamera.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<GameplayCamera>>();

            Container
                .BindFactory<string, UniTask<Enemy>, Enemy.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<Enemy>>();
        }
    }
}
