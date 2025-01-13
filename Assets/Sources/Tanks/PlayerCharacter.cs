using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Tanks
{
    public class PlayerCharacter : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, Transform, UniTask<PlayerCharacter>>
        {
        }
    }
}
