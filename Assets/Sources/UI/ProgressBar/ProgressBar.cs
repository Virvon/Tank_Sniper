using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Level.Sequence;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using MPUIKIT;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Color _completedLevelColor;
        [SerializeField] private Color _currentLevelColor;
        [SerializeField] private Color _uncompletedLevelColor;
        [SerializeField] private MPImage _currentBiomeIcon;
        [SerializeField] private MPImage _nextBiomeIcon;

        private IPersistentProgressService _persistentProgressService;
        private IStaticDataService _staticDataService;
        private IUiFactory _uiFactory;
        private IAssetProvider _assetProvider;

        [Inject]
        private async void Construct(
            IPersistentProgressService persistentProgressService,
            IStaticDataService staticDataService,
            IUiFactory uiFactory,
            IAssetProvider assetProvider)
        {
            _persistentProgressService = persistentProgressService;
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;
            _assetProvider = assetProvider;

            await Fill();
        }

        private async UniTask Fill()
        {
            BiomeType currentBiomeType = _persistentProgressService.Progress.CurrentBiomeType;
            LevelsSequenceConfig levelsSequenceConfig = _staticDataService.GetLevelsSequence(currentBiomeType);
            uint currentLevelIndex = _persistentProgressService.Progress.CurrentLevelIndex;

            int nextBiomeIndex = ((int)currentBiomeType) + 1;

            nextBiomeIndex = nextBiomeIndex >= Enum.GetValues(typeof(BiomeType)).Length ? 0 : nextBiomeIndex;
            BiomeType nextBiomeType = (BiomeType)nextBiomeIndex;

            for (int i = 0; i < levelsSequenceConfig.Sequence.Length; i++)
            {
                ProgressBarElement progressBarElement = await _uiFactory.CreateProgressBarElement(transform);

                Color color;

                if (i < currentLevelIndex)
                    color = _completedLevelColor;
                else if (i > currentLevelIndex)
                    color = _uncompletedLevelColor;
                else
                    color = _currentLevelColor;

                progressBarElement.Initialize(color);
            }

            _currentBiomeIcon.sprite = await _assetProvider.Load<Sprite>(levelsSequenceConfig.IconReference);
            _nextBiomeIcon.sprite = await _assetProvider.Load<Sprite>(_staticDataService.GetLevelsSequence(nextBiomeType).IconReference);
        }
    }
}
