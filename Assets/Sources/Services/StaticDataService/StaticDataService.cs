using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.Level;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IAssetProvider _assetsProvider;

        private Dictionary<string, LevelConfig> _levelConfigs;
        private Dictionary<BulletType, BulletConfig> _bulletConfigs;

        public StaticDataService(IAssetProvider assetsProvider) =>
            _assetsProvider = assetsProvider;

        public async UniTask InitializeAsync()
        {
            List<UniTask> tasks = new()
            {
                UniTask.Create(async () => _levelConfigs = await LoadConfigs<string, LevelConfig>()),
                UniTask.Create(async () => _bulletConfigs = await LoadConfigs<BulletType, BulletConfig>())
            };

            await UniTask.WhenAll(tasks);
        }

        public LevelConfig GetLevel(string key) =>
            _levelConfigs.TryGetValue(key, out LevelConfig config) ? config : null;

        public BulletConfig GetBullet(BulletType type) =>
            _bulletConfigs.TryGetValue(type, out BulletConfig config) ? config : null;

        private async UniTask<Dictionary<TKey, TConfig>> LoadConfigs<TKey, TConfig>()
            where TConfig : class, IConfig<TKey> 
        {
            TConfig[] configs = await GetConfigs<TConfig>();

            return configs.ToDictionary(value => value.Key, value => value);
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
    public interface IConfig<TKey>
    {
        public TKey Key { get; }
    }
}