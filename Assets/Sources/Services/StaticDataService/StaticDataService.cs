using System.Collections.Generic;
using System.Linq;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Services.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IAssetProvider _assetsProvider;

        private Dictionary<string, LevelConfig> _levelConfigs;

        public StaticDataService(IAssetProvider assetsProvider) =>
            _assetsProvider = assetsProvider;

        public async UniTask InitializeAsync()
        {
            List<UniTask> tasks = new();

            tasks.Add(LoadLevelConfigs());

            await UniTask.WhenAll(tasks);
        }

        public LevelConfig GetLevel(string key) =>
            _levelConfigs.TryGetValue(key, out LevelConfig config) ? config : null;

        private async UniTask LoadLevelConfigs()
        {
            LevelConfig[] levelConfigs = await GetConfigs<LevelConfig>();

            _levelConfigs = levelConfigs.ToDictionary(value => value.LevelKey, value => value);
        }

        private async UniTask<TConfig[]> GetConfigs<TConfig>()
            where TConfig : class
        {
            List<string> keys = await GetConfigKeys<TConfig>();
            return await _assetsProvider.LoadAll<TConfig>(keys);
        }

        private async UniTask<List<string>> GetConfigKeys<TConfig>() =>
            await _assetsProvider.GetAssetsListByLabel<TConfig>(AssetLabels.Configs);
    }
}