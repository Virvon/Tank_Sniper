using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.MainMenu
{
    public class Tank : MonoBehaviour
    {
        [SerializeField] private TankSkin _tankSkin;

        public uint Level { get; private set; }

        public void Initialize(uint level, Material skinMaterial)
        {
            Level = level;
            _tankSkin.SetMaterial(skinMaterial);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, Transform, UniTask<Tank>>
        {
        }
    }
}
