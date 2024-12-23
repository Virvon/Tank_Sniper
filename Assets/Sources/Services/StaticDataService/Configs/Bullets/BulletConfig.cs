using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    public class BulletConfig : ScriptableObject
    {
        public AssetReferenceGameObject AssetReference;
        public float ExplosionLifeTime;
        public uint ExplosionForce;
        public float ExplosionRadius;
    }
}