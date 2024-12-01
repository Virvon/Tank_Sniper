﻿using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.Level;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Sources.Services.StaticDataService
{
    public interface IStaticDataService
    {
        WeaponConfig GetWeapon(WeaponType type);
        EnemyConfig GetEnemy(EnemyType type);
        LevelConfig GetLevel(string key);
        UniTask InitializeAsync();
    }
}