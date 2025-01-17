﻿using Assets.Sources.Services.AssetManagement;
using Assets.Sources.UI;
using Assets.Sources.UI.Gameplay.Aim;
using Assets.Sources.UI.Gameplay.BulletsPanel;
using Assets.Sources.UI.MainMenu.Store;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.UiFactory
{
    public class UiFactoryInstaller : Installer<UiFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IUiFactory>()
                .To<UiFactory>()
                .AsSingle();

            Container
                .BindFactory<string, UniTask<Window>, Window.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<Window>>();
            
            Container
                .BindFactory<string, Transform, UniTask<SelectingPanelElement>, SelectingPanelElement.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<SelectingPanelElement>>();
            
            Container
                .BindFactory<string, Transform, UniTask<ProgressBarElement>, ProgressBarElement.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<ProgressBarElement>>();
            
            Container
                .BindFactory<string, Transform, UniTask<BulletIcon>, BulletIcon.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<BulletIcon>>();
            
            Container
                .BindFactory<string, Transform, UniTask<SuperBulletIcon>, SuperBulletIcon.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<SuperBulletIcon>>();
        }
    }
}
