using Assets.Sources.Gameplay.Enemies;
using System;
using System.Collections.Generic;

namespace Assets.Sources.Gameplay.Handlers
{
    public class WictoryHandler : IDisposable
    {
        private List<Enemy> _enemies;
        private int _destructedEnemiesCount;

        public WictoryHandler() =>
            _enemies = new();

        public event Action<int> DestructedEnemiesCountChanger;

        public int MaxEnemiesCount { get; private set; }

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
        }
    }
}
