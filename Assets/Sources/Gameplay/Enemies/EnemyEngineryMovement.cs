using Assets.Sources.Services.StaticDataService.Configs.Level;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies
{
    public class EnemyEngineryMovement : EnemyMovement
    {
        private bool _isLooped;
        private bool _isWaitedAttack;

        private PlayerTank _playerTank;

        private EnemyCarPointConfig _enemyCarPointConfig;

        [Inject]
        private void Construct(PlayerTank playerTank)
        {
            _playerTank = playerTank;

            _playerTank.Attacked += OnPlayerTankAttacked;
        }

        public void Initialize(EnemyCarPointConfig enemyCarPointConfig)
        {
            _enemyCarPointConfig = enemyCarPointConfig;

            _isLooped = _enemyCarPointConfig.IsLooped;
            _isWaitedAttack = _enemyCarPointConfig.IsWaitedAttack;

            Speed = _enemyCarPointConfig.Speed;
            Path = _enemyCarPointConfig.Path;
            MaxRotationAngle = _enemyCarPointConfig.MaxRotationAngle;
        }

        private void Start()
        {
            if (_isWaitedAttack == false)
                StartMovement();
        }

        private void OnDestroy() =>
            _playerTank.Attacked -= OnPlayerTankAttacked;

        private void OnPlayerTankAttacked()
        {
            if (_isWaitedAttack)
                StartMovement();
        }

        protected override bool CanMoveNextCircle() =>
            _isLooped;
    }
}
