using Assets.Sources.Gameplay;
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
        }
    }
}
