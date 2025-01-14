using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class TankBuyingData
    {
        public uint RequiredForPriceUpdateBuyingCount;
        public uint RequiredForSpawnUpdateBuyingCount;
        public uint CurrentCost;
        public uint CurrentSpawnLevel;

        public TankBuyingData(uint startCost)
        {
            CurrentCost = startCost;

            CurrentSpawnLevel = 1;
        }

        public event Action CostUpdated;
        public event Action SpawnLevelUpdated;
        public event Action BuyingCountUpdated;

        public void UpdateInfo()
        {
            RequiredForPriceUpdateBuyingCount++;
            RequiredForSpawnUpdateBuyingCount++;
            BuyingCountUpdated?.Invoke();
        }

        public void UpdatePrice(uint priceIncrease)
        {
            CurrentCost += priceIncrease;
            RequiredForPriceUpdateBuyingCount = 0;
            CostUpdated?.Invoke();
        }
        
        public void UpdateSpawnLevel()
        {
            CurrentSpawnLevel++;
            RequiredForSpawnUpdateBuyingCount = 0;
            SpawnLevelUpdated?.Invoke();
        }
    }
}
