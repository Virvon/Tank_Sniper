using Assets.Sources.Data;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.UI.MainMenu.Store
{
    public class TankSelectingPanel : SelectionPanel<uint>
    {
        protected override void Unsubscribe(IPersistentProgressService persistentProgressService) =>
            persistentProgressService.Progress.TankUnlocked -= Unlock;

        protected override void Subscribe(IPersistentProgressService persistentProgressService) =>
            persistentProgressService.Progress.TankUnlocked += Unlock;

        protected override async UniTask<Dictionary<uint, SelectingPanelElement>> FillContent(
            IUiFactory uiFactory,
            IPersistentProgressService persistentProgressService,
            Transform content)
        {
            Dictionary<uint, SelectingPanelElement> panels = new();
            IOrderedEnumerable<TankData> tankDatas = persistentProgressService.Progress.Tanks.OrderBy(data => data.Level);

            foreach (TankData tankData in tankDatas)
            {
                SelectingPanelElement tankPanel = await uiFactory.CreateTankPanel(content);

                //tankPanel.Initialize(tankData.Level.ToString());

                if (tankData.IsUnlocked)
                    tankPanel.Unlock();

                tankPanel.Clicked += OnPanelClicked;

                panels.Add(tankData.Level, tankPanel);
            }

            return panels;
        }

        protected override void Select(uint key, IPersistentProgressService persistentProgressService) =>
            persistentProgressService.Progress.TrySelectTank(key);

        protected override uint GetCurrentSelectedPanel(IPersistentProgressService persistentProgressService) =>
            persistentProgressService.Progress.GetSelectedTank().Level;
    }
}
