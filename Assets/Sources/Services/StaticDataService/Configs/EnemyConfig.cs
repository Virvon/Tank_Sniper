using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Create new enemy config", order = 51)]
    public class EnemyConfig : ScriptableObject, IConfig<EnemyType>
    {
        public EnemyType Type;
        public AssetReferenceGameObject AssetReference;

        public EnemyType Key => Type;
    }
}