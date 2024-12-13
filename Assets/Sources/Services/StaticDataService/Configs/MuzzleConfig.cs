using Assets.Sources.Types;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "MuzzleConfig", menuName = "Configs/Create new muzzle config", order = 51)]
    public class MuzzleConfig : ScriptableObject, IConfig<MuzzleType>
    {
        public MuzzleType Type;
        public AssetReferenceGameObject AssetReference;
        public float LifeTime;

        public MuzzleType Key => Type;

    }
}