using Assets.Sources.Data;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.MainMenu.Store
{
    public class TankSelectingPanel : SelectionPanel<uint>
    {
        [SerializeField] private Image _icon;

        private IStaticDataService _staticDataService;
        private IAssetProvider _assetProvider;

        [Inject]
        private void Construct(IStaticDataService staticDataService, IAssetProvider assetProvider)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
        }

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

                Sprite icon = await _assetProvider.Load<Sprite>(_staticDataService.GetTank(tankData.Level).Icon);

                tankPanel.Initialize(icon);

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
