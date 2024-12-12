using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Types;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Assets.Sources.MainMenu
{
    public class Decals : MonoBehaviour
    {
        [SerializeField] private DecalProjector[] _decalProjectors;

        private IPersistentProgressService _persistentProgressService;
        private IStaticDataService _staticDataService;
        private IAssetProvider _assetProvider;

        private bool _isChangable;

        [Inject]
        private void Construct(
            IPersistentProgressService persistentProgressService,
            IStaticDataService staticDataService,
            IAssetProvider assetProvider)
        {
            _persistentProgressService = persistentProgressService;
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;

            _isChangable = true;

            _persistentProgressService.Progress.DecalChanged += ChangeDecal;
        }

        private void OnDestroy() =>
            _persistentProgressService.Progress.DecalChanged -= ChangeDecal;

        public void Initialize(DecalType decalType, bool isChangable)
        {
            ChangeDecal(decalType);
            _isChangable = isChangable;
        }

        private async void ChangeDecal(DecalType type)
        {
            if (_isChangable == false)
                return;

            Material material = await _assetProvider.Load<Material>(_staticDataService.GetDecal(type).MaterialAssetReference);

            foreach (DecalProjector decalProjector in _decalProjectors)
                decalProjector.material = material;
        }
    }
}
