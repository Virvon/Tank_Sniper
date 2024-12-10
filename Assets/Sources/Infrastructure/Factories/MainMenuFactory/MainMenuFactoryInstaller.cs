﻿using Assets.Sources.MainMenu;
using Assets.Sources.Services.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.MainMenuFactory
{
    public class MainMenuFactoryInstaller : Installer<MainMenuFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MainMenuFactory>().AsSingle();

            Container
                .BindFactory<string, Vector3, Quaternion, Transform, UniTask<Tank>, Tank.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<Tank>>();
            
            Container
                .BindFactory<string, UniTask<Desk>, Desk.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<Desk>>();
        }
    }
}