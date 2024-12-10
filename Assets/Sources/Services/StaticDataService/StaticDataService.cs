﻿using System.Collections.Generic;
using System.Linq;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.Bullets.Colliding;
using Assets.Sources.Services.StaticDataService.Configs.Bullets.Laser;
using Assets.Sources.Services.StaticDataService.Configs.Level;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Services.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IAssetProvider _assetsProvider;

        private Dictionary<string, LevelConfig> _levelConfigs;
        private Dictionary<EnemyType, EnemyConfig> _enemyConfigs;
        private Dictionary<ForwardFlyingBulletType, ForwardFlyingBulletConfig> _forwardFlyingBulletConfigs;
        private Dictionary<HomingBulletType, HomingBulletConfig> _homingBulletConfigs;
        private Dictionary<uint, TankConfig> _tankConfigs;

        public StaticDataService(IAssetProvider assetsProvider) =>
            _assetsProvider = assetsProvider;

        public LaserConfig DiretionalLaserConfig { get; private set; }
        public TargetingLaserConfig TargetingLaserConfig { get; private set; }
        public TransmittingLaserConfig TransmittedLaserConfig { get; private set; }

        public async UniTask InitializeAsync()
        {
            List<UniTask> tasks = new()
            {
                UniTask.Create(async () => _levelConfigs = await LoadConfigs<string, LevelConfig>()),
                UniTask.Create(async () => _enemyConfigs = await LoadConfigs<EnemyType, EnemyConfig>()),
                UniTask.Create(async () => _forwardFlyingBulletConfigs = await LoadConfigs<ForwardFlyingBulletType, ForwardFlyingBulletConfig>()),
                UniTask.Create(async () => _homingBulletConfigs = await LoadConfigs<HomingBulletType, HomingBulletConfig>()),
                UniTask.Create(async () => DiretionalLaserConfig = await LoadConfig<LaserConfig>()),
                UniTask.Create(async () => TargetingLaserConfig = await LoadConfig<TargetingLaserConfig>()),
                UniTask.Create(async () => TransmittedLaserConfig = await LoadConfig<TransmittingLaserConfig>()),
                UniTask.Create(async () => _tankConfigs = await LoadConfigs<uint, TankConfig>()),
            };

            await UniTask.WhenAll(tasks);
        }

        public TankConfig GetTank(uint level) =>
            _tankConfigs.TryGetValue(level, out TankConfig config) ? config : null;

        public HomingBulletConfig GetBullet(HomingBulletType type) =>
            _homingBulletConfigs.TryGetValue(type, out HomingBulletConfig config) ? config : null;

        public ForwardFlyingBulletConfig GetBullet(ForwardFlyingBulletType type) =>
            _forwardFlyingBulletConfigs.TryGetValue(type, out ForwardFlyingBulletConfig config) ? config : null;

        public EnemyConfig GetEnemy(EnemyType type) =>
            _enemyConfigs.TryGetValue(type, out EnemyConfig config) ? config : null;

        public LevelConfig GetLevel(string key) =>
            _levelConfigs.TryGetValue(key, out LevelConfig config) ? config : null;

        private async UniTask<TConfig> LoadConfig<TConfig>()
            where TConfig : class
        {
            TConfig[] configs = await GetConfigs<TConfig>();

            return configs.FirstOrDefault();
        }

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
}