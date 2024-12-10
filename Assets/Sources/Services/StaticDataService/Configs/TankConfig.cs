using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "TankConfig", menuName = "Configs/Create new tank config", order = 51)]
    public class TankConfig : ScriptableObject, IConfig<uint>
    {
        public uint Level;
        public AssetReferenceGameObject AssetReference;

        public uint Key => Level;
    }
}