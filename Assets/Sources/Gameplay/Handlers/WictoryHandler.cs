using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Services.CoroutineRunner;
using Assets.Sources.Services.InputService;
using Assets.Sources.Services.StaticDataService;
using System;
using System.Collections.Generic;

namespace Assets.Sources.Gameplay.Handlers
{
    public class WictoryHandler : GameplayHandler, IDisposable
    {
        private readonly IInputService _inputService;

        private List<Enemy> _enemies;
        private int _destructedEnemiesCount;

        public WictoryHandler(ICoroutineRunner coroutineRunner, IStaticDataService staticDataService, IInputService inputService)
            : base(coroutineRunner, staticDataService)
        {
            _enemies = new();
            _inputService = inputService;
        }

        public event Action<int> DestructedEnemiesCountChanger;
        public event Action Woned;
        public event Action WindowsSwithed;

        public int MaxEnemiesCount { get; private set; }
        public bool IsWoned => _destructedEnemiesCount >= MaxEnemiesCount;

        public void AddEnemy(Enemy enemy)
        {
            _enemies.Add(enemy);
            enemy.Destructed += OnEnemyDestructed;

            MaxEnemiesCount++;
        }

        public void Dispose()
        {
            foreach (Enemy enemy in _enemies)
                enemy.Destructed -= OnEnemyDestructed;
        }

        private void OnEnemyDestructed()
        {
            _destructedEnemiesCount++;
            DestructedEnemiesCountChanger?.Invoke(_destructedEnemiesCount);

            if(IsWoned)
            {
                Woned?.Invoke();
                _inputService.SetActive(false);
                StartTimer(callback: () => WindowsSwithed?.Invoke());
            }
        }
    }
}