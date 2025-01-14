using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using MPUIKIT;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.MainMenu
{
    public class TankBuyingProgressBar : MonoBehaviour
    {
        private const string Info = "УРОВЕНЬ";

        [SerializeField] private MPImage _fill;
        [SerializeField] private TMP_Text _levelValue;

        private IPersistentProgressService _persistentPorgressService;
        private IStaticDataService _staticDataService;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, IStaticDataService staticDataService)
        {
            _persistentPorgressService = persistentProgressService;
            _staticDataService = staticDataService;

            OnTanksBuyingCountUpdated();
            OnTankSpawnLevelUpdated();

            _persistentPorgressService.Progress.TankBuyingData.SpawnLevelUpdated += OnTankSpawnLevelUpdated;
            _persistentPorgressService.Progress.TankBuyingData.BuyingCountUpdated += OnTanksBuyingCountUpdated;
        }

        private void OnDestroy()
        {
            _persistentPorgressService.Progress.TankBuyingData.SpawnLevelUpdated -= OnTankSpawnLevelUpdated;
            _persistentPorgressService.Progress.TankBuyingData.BuyingCountUpdated -= OnTanksBuyingCountUpdated;
        }

        private void OnTanksBuyingCountUpdated() =>
            _fill.fillAmount = (float)_persistentPorgressService.Progress.TankBuyingData.RequiredForSpawnUpdateBuyingCount / (float)_staticDataService.MainMenuSettingsConfig.RequiredTanksNumberToSpawnUpdate;

        private void OnTankSpawnLevelUpdated() =>
            _levelValue.text = $"{Info} {_persistentPorgressService.Progress.TankBuyingData.CurrentSpawnLevel}";
    }
}
