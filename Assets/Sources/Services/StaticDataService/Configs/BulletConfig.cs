using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "BulletConfig", menuName = "Configs/Create new bullet config", order = 51)]
    public class BulletConfig : ScriptableObject, IConfig<BulletType>
    {
        public BulletType Type;
        public AssetReferenceGameObject AssetReference;

        public BulletType Key => Type;
    }
}