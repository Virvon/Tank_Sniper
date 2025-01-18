using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using System.Linq;

namespace Assets.Sources.MainMenu.TanksBuying
{
    public class TankBuyer
    {
        private IPersistentProgressService _persistentProgressService;
        private MainMenuSettingsConfig _mainMenuSettingsConfig;

        public TankBuyer(IPersistentProgressService persistentProgressService, IStaticDataService staticDataService)
        {
            _persistentProgressService = persistentProgressService;
            _mainMenuSettingsConfig = staticDataService.MainMenuSettingsConfig;
        }

        public bool TryBuyTank(out uint tankLevel)
        {
            tankLevel = _persistentProgressService.Progress.TankBuyingData.CurrentSpawnLevel;

            if (_persistentProgressService.Progress.Wallet.TryTake(_persistentProgressService.Progress.TankBuyingData.CurrentCost))
            {
                _persistentProgressService.Progress.TankBuyingData.UpdateInfo();


                if (_persistentProgressService.Progress.TankBuyingData.RequiredForPriceUpdateBuyingCount > _mainMenuSettingsConfig.RequiredTanksNumberToPriceUpdate)
                    _persistentProgressService.Progress.TankBuyingData.UpdatePrice(_mainMenuSettingsConfig.PriceIncrease);

                if(_persistentProgressService.Progress.TankBuyingData.RequiredForSpawnUpdateBuyingCount >= _mainMenuSettingsConfig.RequiredTanksNumberToSpawnUpdate)
                {
                    uint currentSpawnLevel = _persistentProgressService.Progress.TankBuyingData.CurrentSpawnLevel;
                    float count = 1;
                    float divider = 1;

                    for (uint i = currentSpawnLevel; i >= 1; i--)
                    {
                        count += (float)_persistentProgressService.Progress.DeskData.Cells.Count(celldata => celldata.TankLevel == i) / divider;
                        divider *= 2;
                    }

                    if (count % 2 == 0)
                        _persistentProgressService.Progress.TankBuyingData.UpdateSpawnLevel();
                }

                return true;
            }

            return false;
        }
    }
}
