using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Services.AssetManagement
{
    public class ReferencePrefabFactoryAsync<TComponent> : IFactory<AssetReferenceGameObject, UniTask<TComponent>>,
        IFactory<AssetReferenceGameObject, Vector3, Transform, UniTask<TComponent>>,
        IFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<TComponent>>,
        IFactory<AssetReferenceGameObject, Transform, UniTask<TComponent>>,
        IFactory<AssetReferenceGameObject, Vector3, UniTask<TComponent>>
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IInstantiator _instantiator;

        public ReferencePrefabFactoryAsync(IAssetProvider assetProvider, IInstantiator instantiator)
        {
            _assetProvider = assetProvider;
            _instantiator = instantiator;
        }

        public async UniTask<TComponent> Create(AssetReferenceGameObject assetReference)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(assetReference);
            GameObject newObject = _instantiator.InstantiatePrefab(prefab);
            return newObject.GetComponent<TComponent>();
        }

        public async UniTask<TComponent> Create(AssetReferenceGameObject assetReference, Vector3 position, Transform parent)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(assetReference);
            GameObject newObject = _instantiator.InstantiatePrefab(prefab, position, Quaternion.identity, parent);
            return newObject.GetComponent<TComponent>();
        }

        public async UniTask<TComponent> Create(AssetReferenceGameObject assetReference, Vector3 position, Quaternion rotation)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(assetReference);
            GameObject newObject = _instantiator.InstantiatePrefab(prefab, position, rotation, null);
            return newObject.GetComponent<TComponent>();
        }

        public async UniTask<TComponent> Create(AssetReferenceGameObject assetReference, Transform parent)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(assetReference);
            GameObject newObject = _instantiator.InstantiatePrefab(prefab, parent);
            return newObject.GetComponent<TComponent>();
        }

        public async UniTask<TComponent> Create(AssetReferenceGameObject assetReference, Vector3 position)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(assetReference);
            GameObject newObject = _instantiator.InstantiatePrefab(prefab);
            newObject.transform.position = position;
            return newObject.GetComponent<TComponent>();
        }
    }
}