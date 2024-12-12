using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "TankConfig", menuName = "Configs/Create new tank config", order = 51)]
    public class TankConfig : ScriptableObject, IConfig<uint>
    {
        public uint Level;
        public AssetReferenceGameObject AssetReference;
        public bool IsUnlockOnStart;
        public AssetReference BaseMaterialAssetReference;

        public uint Key => Level;
    }
}