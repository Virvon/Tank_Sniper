﻿using Assets.Sources.Gameplay.Player;
using Assets.Sources.MainMenu;
using Assets.Sources.Tanks;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.TankFactory
{
    public interface ITankFactory
    {
        UniTask<PlayerTankWrapper> CreatePlayerTankWrapper(uint tankLevel, Vector3 position, Quaternion rotation);
        UniTask<Tank> CreateTank(
            uint level,
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            TankSkinType skinType = TankSkinType.Base,
            DecalType decalType = DecalType.Decal1,
            bool isDecalsChangable = false);
        UniTask<TankShootingWrapper> CreateTankShootingWrapper(uint tankLevel, Vector3 position, Quaternion rotation, Transform parent);
    }
}