﻿using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Assets.Sources.Services.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _assetRequests;

        public AssetProvider() =>
            _assetRequests = new ();

        public async UniTask InitializeAsync()
        {
            await Addressables.InitializeAsync().ToUniTask();
        }

        public void CleanUp()
        {
            foreach (AsyncOperationHandle handle in _assetRequests.Values)
                Addressables.Release(handle);

            _assetRequests.Clear();
        }

        public async UniTask<TAsset> Load<TAsset>(AssetReference reference)
        {
            AsyncOperationHandle handle;

            if (_assetRequests.TryGetValue(reference.AssetGUID, out handle) == false)
            {
                handle = reference.LoadAssetAsync<TAsset>();
                _assetRequests.Add(reference.AssetGUID, handle);
            }

            await handle.ToUniTask();

            return (TAsset)handle.Result;
        }

        public async UniTask<TAsset> Load<TAsset>(AssetReferenceGameObject reference)
            where TAsset : class
        {
            AsyncOperationHandle handle;

            if (_assetRequests.TryGetValue(reference.AssetGUID, out handle) == false)
            {
                handle = Addressables.LoadAssetAsync<TAsset>(reference);
                _assetRequests.Add(reference.AssetGUID, handle);
            }

            await handle.ToUniTask();

            return handle.Result as TAsset;
        }

        public async UniTask<TAsset> Load<TAsset>(string key)
            where TAsset : class
        {
            AsyncOperationHandle handle;

            if (_assetRequests.TryGetValue(key, out handle) == false)
            {
                handle = Addressables.LoadAssetAsync<TAsset>(key);
                _assetRequests.Add(key, handle);
            }

            await handle.ToUniTask();

            return handle.Result as TAsset;
        }

        public async UniTask WarmupAssetsByLable(string label)
        {
            List<string> assetsList = await GetAssetsListByLabel(label);
            await LoadAll<object>(assetsList);
        }

        public async UniTask ReleaseAssetsByLabel(string label)
        {
            var assetsList = await GetAssetsListByLabel(label);

            foreach (var assetKey in assetsList)
            {
                if (_assetRequests.TryGetValue(assetKey, out var handler))
                {
                    Addressables.Release(handler);
                    _assetRequests.Remove(assetKey);
                }
            }
        }

        public async UniTask<TAsset[]> LoadAll<TAsset>(List<string> keys)
            where TAsset : class
        {
            List<UniTask<TAsset>> tasks = new (keys.Count);

            foreach (string key in keys)
                tasks.Add(Load<TAsset>(key));

            return await UniTask.WhenAll(tasks);
        }

        public async UniTask<List<string>> GetAssetsListByLabel<TAsset>(string label) =>
            await GetAssetsListByLabel(label, typeof(TAsset));

        private async UniTask<List<string>> GetAssetsListByLabel(string label, Type type = null)
        {
            AsyncOperationHandle<IList<IResourceLocation>> operationHandle = Addressables.LoadResourceLocationsAsync(label, type);
            IList<IResourceLocation> locations = await operationHandle.ToUniTask();

            List<string> assetKeys = new List<string>(locations.Count);

            foreach (IResourceLocation location in locations)
                assetKeys.Add(location.PrimaryKey);

            Addressables.Release(operationHandle);
            return assetKeys;
        }
    }
}