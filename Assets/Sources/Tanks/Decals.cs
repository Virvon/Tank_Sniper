using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Assets.Sources.Tanks
{
    public class Decals : MonoBehaviour
    {
        [SerializeField] private DecalProjector[] _decalProjectors;
        [SerializeField] private DecalScalingAnimator[] _decalAnimators;

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

            _persistentProgressService.Progress.DecalChanged += OnDecalChanged;
        }

        private void OnDestroy() =>
            _persistentProgressService.Progress.DecalChanged -= OnDecalChanged;

        public async void Initialize(string id, bool isChangable)
        {
            await ChangeDecal(id, false);
            _isChangable = isChangable;
        }

        private async UniTask ChangeDecal(string id, bool needToAnimate)
        {
            if (_isChangable == false)
                return;

            Material material = await _assetProvider.Load<Material>(_staticDataService.GetDecal(id).MaterialAssetReference);

            foreach (DecalProjector decalProjector in _decalProjectors)
                decalProjector.material = material;

            if (needToAnimate)
            {
                foreach (DecalScalingAnimator decalScalingAnimator in _decalAnimators)
                    decalScalingAnimator.Play();
            }
        }

        private async void OnDecalChanged(string id) =>
            await ChangeDecal(id, true);
    }
}
