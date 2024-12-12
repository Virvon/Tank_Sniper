﻿using Assets.Sources.MainMenu;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.MainMenuFactory
{
    public interface IMainMenuFactory
    {
        UniTask CreateDesk();
        UniTask<Tank> CreateTank(uint level, Vector3 position, Quaternion rotation, Transform parent, TankSkinType skinType = TankSkinType.Base);
    }
}