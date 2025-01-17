﻿using Assets.Sources.MainMenu.Desk;
using Assets.Sources.Services.AssetManagement;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.MainMenuFactory
{
    public class MainMenuFactoryInstaller : Installer<MainMenuFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MainMenuFactory>().AsSingle();
            
            Container
                .BindFactory<string, UniTask<Desk>, Desk.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<Desk>>();
        }
    }
}
