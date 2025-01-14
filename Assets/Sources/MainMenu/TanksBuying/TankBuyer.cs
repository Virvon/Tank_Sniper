using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;

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

                if(_persistentProgressService.Progress.TankBuyingData.RequiredForSpawnUpdateBuyingCount >= _mainMenuSettingsConfig.RequiredTanksNumberToSpawnUpdate)
                    _persistentProgressService.Progress.TankBuyingData.UpdateSpawnLevel();

                if (_persistentProgressService.Progress.TankBuyingData.RequiredForPriceUpdateBuyingCount > _mainMenuSettingsConfig.RequiredTanksNumberToPriceUpdate)
                    _persistentProgressService.Progress.TankBuyingData.UpdatePrice(_mainMenuSettingsConfig.PriceIncrease);

                return true;
            }

            return false;
        }
    }
}
