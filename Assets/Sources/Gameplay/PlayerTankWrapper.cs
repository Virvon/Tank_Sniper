using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class PlayerTankWrapper : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<PlayerTankWrapper>>
        {
        }
    }
}
