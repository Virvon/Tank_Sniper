using Assets.Sources.Types;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "TankSkinConfig", menuName = "Configs/Create new tank skin config", order = 51)]
    public class TankSkinConfig : ScriptableObject, IConfig<TankSkinType>
    {
        public TankSkinType Type;
        public AssetReference Material;

        public TankSkinType Key => Type;
    }
}