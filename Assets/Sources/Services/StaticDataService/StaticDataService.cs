using System.Collections.Generic;
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
        private Dictionary<TankSkinType, TankSkinConfig> _tankSkinConfigs;
        private Dictionary<DecalType, DecalConfig> _decalConfigs;
        private Dictionary<MuzzleType, MuzzleConfig> _muzzleConfigs;

        public StaticDataService(IAssetProvider assetsProvider) =>
            _assetsProvider = assetsProvider;

        public LaserConfig DiretionalLaserConfig { get; private set; }
        public TargetingLaserConfig TargetingLaserConfig { get; private set; }
        public TransmittingLaserConfig TransmittedLaserConfig { get; private set; }
        public TankConfig[] TankConfigs => _tankConfigs.Values.ToArray();
        public TankSkinConfig[] TankSkinConfigs => _tankSkinConfigs.Values.ToArray();
        public DecalConfig[] DecalConfigs => _decalConfigs.Values.ToArray();
        public AnimationsConfig AnimationsConfig { get; private set; }
        public AimingConfig AimingConfig { get; private set; }
        public DestructionConfig DestructionConfig { get; private set; }
        public EnemiesSettingsConfig EnemiesSettingsConfig { get; private set; }

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
                UniTask.Create(async () => _tankSkinConfigs = await LoadConfigs<TankSkinType, TankSkinConfig>()),
                UniTask.Create(async () => _decalConfigs = await LoadConfigs<DecalType, DecalConfig>()),
                UniTask.Create(async () => _muzzleConfigs = await LoadConfigs<MuzzleType, MuzzleConfig>()),
                UniTask.Create(async () => AnimationsConfig = await LoadConfig<AnimationsConfig>()),
                UniTask.Create(async () => AimingConfig = await LoadConfig<AimingConfig>()),
                UniTask.Create(async () => DestructionConfig = await LoadConfig<DestructionConfig>()),
                UniTask.Create(async () => EnemiesSettingsConfig = await LoadConfig<EnemiesSettingsConfig>()),
            };

            await UniTask.WhenAll(tasks);
        }

        public MuzzleConfig GetMuzzle(MuzzleType type) =>
            _muzzleConfigs.TryGetValue(type, out MuzzleConfig config) ? config : null;

        public DecalConfig GetDecal(DecalType type) =>
            _decalConfigs.TryGetValue(type, out DecalConfig config) ? config : null;

        public TankSkinConfig GetSkin(TankSkinType type) =>
            _tankSkinConfigs.TryGetValue(type, out TankSkinConfig config) ? config : null;

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