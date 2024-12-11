using Assets.Sources.Data;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.MainMenu
{
    public class TanksPanel : MonoBehaviour
    {
        [SerializeField] private Transform _content;

        private IPersistentProgressService _persistentProgressService;
        private IUiFactory _uiFactory;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, IUiFactory uiFactory)
        {
            _persistentProgressService = persistentProgressService;
            _uiFactory = uiFactory;

            FillContent();
        }

        private async void FillContent()
        {
            foreach(TankData tankData in _persistentProgressService.Progress.Tanks)
            {
                TankPanel tankPanel = await _uiFactory.CreateTankPanel(_content);

                tankPanel.Initialize(tankData.Level.ToString());

                if (tankData.IsUnlocked)
                    tankPanel.Unlock();
            }
        }
    }
}
