using Assets.Sources.Types;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Configs/Create new weapon config", order = 51)]
    public class WeaponConfig : ScriptableObject
    {
        public AssetReferenceGameObject BulletAssetReference;
        public uint BulletsCapacity;
        public float ShootCooldown;
        public float ReloadDuration;
    }
}