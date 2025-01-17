﻿using Assets.Sources.Data;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Types;
using Assets.Sources.UI.Gameplay.WictoryWindow;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.MainMenu.Store
{
    public class CharacterSkinSelectionPanel : SelectionPanel<string>
    {
        [SerializeField] private int _tankRotationAngle;
        [SerializeField] private UiSelectedTankPoint _tankPoint;

        private IStaticDataService _staticDataService;
        private IAssetProvider _assetProvider;

        [Inject]
        private void Construct(IStaticDataService staticDataService, IAssetProvider assetProvider)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
        }

        public override void Open()
        {
            base.Open();
            _tankPoint.SetTargetRotation(_tankRotationAngle);
        }

        public override void Hide()
        {
            base.Hide();
            _tankPoint.ResetTargetRotation();
        }

        protected override async UniTask<Dictionary<string, SelectingPanelElement>> FillContent(
            IUiFactory uiFactory,
            IPersistentProgressService persistentProgressService,
            Transform content)
        {
            Dictionary<string, SelectingPanelElement> panels = new();

            PlayerCharacterData[] datas = persistentProgressService.Progress.PlayerCharacters.Where(data => data.IsUnlocked).ToArray();

            foreach (PlayerCharacterData skinData in datas)
            {
                SelectingPanelElement panel = await uiFactory.CreateCharacterSkinPanel(content);

                Sprite icon = await _assetProvider.Load<Sprite>(_staticDataService.GetPlayerCharacter(skinData.Id).Icon);

                panel.Initialize(icon);

                if (skinData.IsBuyed)
                    panel.Unlock();

                if (skinData.IsUnlocked)
                    ((PlayerCharacterPanel)panel).RemoveBackground();

                panel.Clicked += OnPanelClicked;

                panels.Add(skinData.Id, panel);
            }

            datas = persistentProgressService.Progress.PlayerCharacters.Where(data => data.IsUnlocked == false).ToArray();

            foreach (PlayerCharacterData skinData in datas)
            {
                SelectingPanelElement panel = await uiFactory.CreateCharacterSkinPanel(content);

                Sprite icon = await _assetProvider.Load<Sprite>(_staticDataService.GetPlayerCharacter(skinData.Id).Icon);

                panel.Initialize(icon);

                if (skinData.IsBuyed)
                    panel.Unlock();

                if (skinData.IsUnlocked)
                    ((PlayerCharacterPanel)panel).RemoveBackground();

                panel.Clicked += OnPanelClicked;

                panels.Add(skinData.Id, panel);
            }

            return panels;
        }

        protected override void Select(string key, IPersistentProgressService persistentProgressService)
        {
            PlayerCharacterData skinData = persistentProgressService.Progress.GetPlayerCharacter(key);

            if(skinData.IsBuyed == false)
            {
#if !UNITY_WEBGL || UNITY_EDITOR
                persistentProgressService.Progress.BuyCharacterSkin(key);
#else
            Agava.YandexGames.InterstitialAd.Show(onCloseCallback: (value) =>
            {
                persistentProgressService.Progress.BuyCharacterSkin(key);
            });
#endif
            }
            else if(skinData.IsUnlocked)
            {
                persistentProgressService.Progress.SelectCharacterSkin(key);
            }

            ActiveSelectionFrame(key);
        }

        protected override void Subscribe(IPersistentProgressService persistentProgressService) =>
            persistentProgressService.Progress.CharacterSkinBuyed += Unlock;

        protected override void Unsubscribe(IPersistentProgressService persistentProgressService) =>
            persistentProgressService.Progress.CharacterSkinBuyed -= Unlock;

        protected override string GetCurrentSelectedPanel(IPersistentProgressService persistentProgressService) =>
            persistentProgressService.Progress.SelectedPlayerCharacterId;
    }
}
