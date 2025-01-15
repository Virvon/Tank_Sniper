using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "TankSkinConfig", menuName = "Configs/Create new tank skin config", order = 51)]
    public class TankSkinConfig : ScriptableObject, IConfig<string>
    {
        public string Id;
        public AssetReference MaterialAssetReference;

        public string Key => Id;
    }
}