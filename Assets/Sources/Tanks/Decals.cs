﻿using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Tanks
{
    public class Decals : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] _renderers;
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

        public void Initialize(Material decalMaterial, bool isChangable)
        {
            ChangeDecalMaterial(decalMaterial);
            _isChangable = isChangable;
        }

        private async UniTask ChangeDecal(string id, bool needToAnimate)
        {
            if (_isChangable == false)
                return;

            Material material = await _assetProvider.Load<Material>(_staticDataService.GetDecal(id).MaterialAssetReference);

            ChangeDecalMaterial(material);

            if (needToAnimate)
            {
                foreach (DecalScalingAnimator decalScalingAnimator in _decalAnimators)
                    decalScalingAnimator.Play();
            }
        }

        private void ChangeDecalMaterial(Material material)
        {
            if(material == null)
            {
                foreach (MeshRenderer renderer in _renderers)
                    renderer.gameObject.SetActive(false);

                return;
            }

            foreach (MeshRenderer renderer in _renderers)
            {
                Material[] materials = renderer.materials;
                materials[0] = material;
                renderer.materials = materials;
            }
        }

        private async void OnDecalChanged(string id) =>
            await ChangeDecal(id, true);
    }
}
