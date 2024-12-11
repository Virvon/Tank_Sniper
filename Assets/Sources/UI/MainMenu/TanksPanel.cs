using Assets.Sources.Data;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.MainMenu
{
    public class TanksPanel : MonoBehaviour
    {
        [SerializeField] private Transform _content;

        private IPersistentProgressService _persistentProgressService;
        private IUiFactory _uiFactory;

        private Dictionary<uint, TankPanel> _tankPanels;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, IUiFactory uiFactory)
        {
            _persistentProgressService = persistentProgressService;
            _uiFactory = uiFactory;

            _tankPanels = new();

            _persistentProgressService.Progress.TankUnlocked += OnTankUnlocked;

            FillContent();
        }

        private void OnDestroy()
        {
            _persistentProgressService.Progress.TankUnlocked -= OnTankUnlocked;

            foreach (TankPanel tankPanel in _tankPanels.Values)
                tankPanel.Clicked -= OnTankPanelClicked;
        }

        private void OnTankUnlocked(uint level) =>
            _tankPanels[level].Unlock();

        private async void FillContent()
        {
            IOrderedEnumerable<TankData> tankDatas = _persistentProgressService.Progress.Tanks.OrderBy(data => data.Level);

            foreach(TankData tankData in tankDatas)
            {
                TankPanel tankPanel = await _uiFactory.CreateTankPanel(_content);

                tankPanel.Initialize(tankData.Level.ToString());

                if (tankData.IsUnlocked)
                    tankPanel.Unlock();

                tankPanel.Clicked += OnTankPanelClicked;

                _tankPanels.Add(tankData.Level, tankPanel);
            }
        }

        private void OnTankPanelClicked(TankPanel panel) =>
            _persistentProgressService.Progress.TrySelectTank(_tankPanels.Keys.First(key => _tankPanels[key] == panel));
    }
}
