using Assets.Sources.Types;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "PlayerCharacterConfig", menuName = "Configs/Create new player character config", order = 51)]
    public class PlayerCharacterConfig : ScriptableObject, IConfig<PlayerCharacterType>
    {
        public PlayerCharacterType Type;
        public bool IsUnlockedOnStart;
        public AssetReferenceGameObject AssetReference;

        public PlayerCharacterType Key => Type;
    }
}