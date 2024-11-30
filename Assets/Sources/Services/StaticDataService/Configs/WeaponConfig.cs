using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Configs/Create new weapon config", order = 51)]
    public class WeaponConfig : ScriptableObject, IConfig<WeaponType>
    {
        public WeaponType Type;
        public AssetReferenceGameObject AssetReference;

        public WeaponType Key => Type;

        public uint BulletsCapacity { get; internal set; }
        public float ShootCooldown { get; internal set; }
        public float ReloadDuration { get; internal set; }
    }
}