﻿using Assets.Sources.Types;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "BulletConfig", menuName = "Configs/Create new bullet config", order = 51)]
    public class BulletConfig : ScriptableObject, IConfig<BulletType>
    {
        public BulletType Type;
        public AssetReferenceGameObject AssetReference;
        public uint ExplosionRadius;
        public uint FlightSpeed;
        public float ExplosionLifeTime;
        public uint ExplosionForce;
        public float ProjectileLifeTime;
        public float SearchRadius;
        public float RotationSpeed;
        public float TargetingDelay;
        public int TargetsCount;
        public uint PartsCount;

        public BulletType Key => Type;

    }
}