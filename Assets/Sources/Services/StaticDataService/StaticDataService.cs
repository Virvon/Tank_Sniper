using System.Collections.Generic;
using System.Linq;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.Bullets.Colliding;
using Assets.Sources.Services.StaticDataService.Configs.Bullets.Laser;
using Assets.Sources.Services.StaticDataService.Configs.Level.EnemyPoints;
using Assets.Sources.Services.StaticDataService.Configs.Level.Sequence;
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
        private Dictionary<string, TankSkinConfig> _tankSkinConfigs;
        private Dictionary<string, DecalConfig> _decalConfigs;
        private Dictionary<MuzzleType, MuzzleConfig> _muzzleConfigs;
        private Dictionary<BiomeType, LevelsSequenceConfig> _levelsSequenceConfigs;
        private Dictionary<PlayerCharacterType, PlayerCharacterConfig> _playerCharacterConfigs;

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
        public GameplaySettingsConfig GameplaySettingsConfig { get; private set; }
        public EnviromentExplosionsConfig EnviromentExplosionsConfig { get; private set; }
        public CompositeBulletConfig CompositeBulletConfig { get; private set; }
        public PlayerCharacterConfig[] PlayerCharacterCofigs => _playerCharacterConfigs.Values.ToArray();
        public MainMenuSettingsConfig MainMenuSettingsConfig { get; private set; }

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
                UniTask.Create(async () => _tankSkinConfigs = await LoadConfigs<string, TankSkinConfig>()),
                UniTask.Create(async () => _decalConfigs = await LoadConfigs<string, DecalConfig>()),
                UniTask.Create(async () => _muzzleConfigs = await LoadConfigs<MuzzleType, MuzzleConfig>()),
                UniTask.Create(async () => AnimationsConfig = await LoadConfig<AnimationsConfig>()),
                UniTask.Create(async () => AimingConfig = await LoadConfig<AimingConfig>()),
                UniTask.Create(async () => DestructionConfig = await LoadConfig<DestructionConfig>()),
                UniTask.Create(async () => GameplaySettingsConfig = await LoadConfig<GameplaySettingsConfig>()),
                UniTask.Create(async () => _levelsSequenceConfigs = await LoadConfigs<BiomeType, LevelsSequenceConfig>()),
                UniTask.Create(async () => EnviromentExplosionsConfig = await LoadConfig<EnviromentExplosionsConfig>()),
                UniTask.Create(async () => CompositeBulletConfig = await LoadConfig<CompositeBulletConfig>()),
                UniTask.Create(async () => _playerCharacterConfigs = await LoadConfigs<PlayerCharacterType, PlayerCharacterConfig>()),
                UniTask.Create(async () => MainMenuSettingsConfig = await LoadConfig<MainMenuSettingsConfig>()),
            };

            await UniTask.WhenAll(tasks);
        }

        public PlayerCharacterConfig GetPlayerCharacter(PlayerCharacterType type) =>
            _playerCharacterConfigs.TryGetValue(type, out PlayerCharacterConfig config) ? config : null;

        public LevelsSequenceConfig GetLevelsSequence(BiomeType type) =>
            _levelsSequenceConfigs.TryGetValue(type, out LevelsSequenceConfig config) ? config : null;

        public MuzzleConfig GetMuzzle(MuzzleType type) =>
            _muzzleConfigs.TryGetValue(type, out MuzzleConfig config) ? config : null;

        public DecalConfig GetDecal(string id) =>
            _decalConfigs.TryGetValue(id, out DecalConfig config) ? config : null;

        public TankSkinConfig GetSkin(string id) =>
            _tankSkinConfigs.TryGetValue(id, out TankSkinConfig config) ? config : null;

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