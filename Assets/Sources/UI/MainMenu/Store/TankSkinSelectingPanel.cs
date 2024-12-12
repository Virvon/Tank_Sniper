using Assets.Sources.Data;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI.MainMenu.Store
{
    public class TankSkinSelectingPanel : SelectingPanel<TankSkinType>
    {
        [SerializeField] private Button _baseSkinButton;

        protected override async UniTask<Dictionary<TankSkinType, SelectingPanelElement>> FillContent(
            IUiFactory uiFactory,
            IPersistentProgressService persistentProgressService,
            Transform content)
        {
            Dictionary<TankSkinType, SelectingPanelElement> panels = new();

            foreach(TankSkinData tankSkinData in persistentProgressService.Progress.TankSkins)
            {
                SelectingPanelElement panel = await uiFactory.CreateUnlockingPanel(content);

                panel.Initialize(tankSkinData.Type.ToString());

                if(tankSkinData.IsUnlocked)
                    panel.Unlock();

                panel.Clicked += OnPanelClicked;

                panels.Add(tankSkinData.Type, panel);
            }

            return panels;
        }

        protected override void Select(TankSkinType key, IPersistentProgressService persistentProgressService)
        {
            TankSkinData tankSkinData = persistentProgressService.Progress.GetSkin(key);

            if(tankSkinData.IsUnlocked == false)
            {
                Debug.Log("Reward video");
                persistentProgressService.Progress.UnlockTankSkin(key);
            }
            else
            {
                persistentProgressService.Progress.SelectTankSkin(key);
            }
        }

        protected override void Subscribe(IPersistentProgressService persistentProgressService)
        {
            persistentProgressService.Progress.TankSkinUnlocked += Unlock;
            _baseSkinButton.onClick.AddListener(OnBaseSkinButtonClicked);
        }

        protected override void Unsubscribe(IPersistentProgressService persistentProgressService)
        {
            persistentProgressService.Progress.TankSkinUnlocked -= Unlock;
            _baseSkinButton.onClick.RemoveListener(OnBaseSkinButtonClicked);
        }

        private void OnBaseSkinButtonClicked() =>
            PersistentProgressService.Progress.SelectTankSkin(TankSkinType.Base);
    }
}
