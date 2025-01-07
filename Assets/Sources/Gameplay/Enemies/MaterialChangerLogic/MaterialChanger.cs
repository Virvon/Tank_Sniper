using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Types;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies.MaterialChangerLogic
{
    public class MaterialChanger : MonoBehaviour
    {
        [SerializeField] private MatchingMaterialToBiome[] _infos;
        [SerializeField] private Renderer _renderer;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService)
        {
            BiomeType currentBiomeType = persistentProgressService.Progress.CurrentBiomeType;

            MatchingMaterialToBiome currentMaterialsInfo = _infos.First(info => info.BiomeType == currentBiomeType);

            Material[] materials = _renderer.materials;

            foreach (MaterialInfo info in currentMaterialsInfo.MaterialInfos)
                materials[info.Index] = info.Material;

            _renderer.materials = materials;
        }
    }
}
