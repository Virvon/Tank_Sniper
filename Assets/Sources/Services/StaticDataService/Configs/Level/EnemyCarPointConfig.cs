﻿using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using System.Linq;
using Assets.Sources.Types;

namespace Assets.Sources.Services.StaticDataService.Configs.Level
{
    [Serializable]
    public class EnemyCarPointConfig : StaticEnemyPointConfig
    {
        public PathPointConfig[] Path;
        public uint MaxRotationAngle;
        public float Speed;
        public bool IsLooped;
        public bool IsWaitedAttack;

        public EnemyCarPointConfig(
            string id,
            EnemyType enemyType,
            EnemyPathPoint[] path,
            Transform startPoint,
            uint maxRotationAngle,
            float speed)
            : base(id, startPoint.position, startPoint.rotation, enemyType)
        {
            Path = path.Select(value => new PathPointConfig(value.transform.position, value.RotationAngle, value.RotationDelta)).ToArray();
            MaxRotationAngle = maxRotationAngle;
            Speed = speed;
        }

        public override async UniTask<Enemy> Create(IGameplayFactory gameplayFactory)
        {
            Enemy enemy = await base.Create(gameplayFactory);

            enemy.GetComponent<EnemyEngineryMovement>().Initialize(this);

            return enemy;
        }
    }
}