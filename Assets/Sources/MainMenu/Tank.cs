using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.MainMenu
{
    public class Tank : MonoBehaviour
    {
        [SerializeField] private TankSkin _tankSkin;
        [SerializeField] private Decals _decals;

        public uint Level { get; private set; }

        public void Initialize(uint level, Material skinMaterial, DecalType decalType, bool isDecalsChangable)
        {
            Level = level;
            _tankSkin.SetMaterial(skinMaterial);
            _decals.Initialize(decalType, isDecalsChangable);
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
