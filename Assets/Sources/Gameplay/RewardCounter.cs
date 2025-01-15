using Assets.Sources.Gameplay.Handlers;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.Gameplay
{
    public class RewardCounter
    {
        private const float MinRewardModifier = 1;

        private readonly WictoryHandler _wictoryHandler;
        private readonly GameplaySettingsConfig _gameplaySettings;
        private readonly IPersistentProgressService _persistentProgerssService;

        public RewardCounter(
            WictoryHandler wictoryHandler,
            IStaticDataService staticDataService,
            IPersistentProgressService persistentProgerssService)
        {
            _wictoryHandler = wictoryHandler;
            _gameplaySettings = staticDataService.GameplaySettingsConfig;
            _persistentProgerssService = persistentProgerssService;
        }

        public uint GetReward()
        {
            int destructionEnemiesCount = _wictoryHandler.Enemies.Count(enemy => enemy.IsDestructed);

            return (uint)(destructionEnemiesCount * _gameplaySettings.RewardPerEnemy * Mathf.Max(
                MinRewardModifier,
                (float)_persistentProgerssService.Progress.CompletedLevelsCount * _gameplaySettings.RewardLevelModifier));
        }
    }
}
