using Assets.Sources.Gameplay.Player;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies.Movement
{
    public class EnemyEngineryMovement : EnemyMovement
    {
        private bool _isPatLooped;
        private bool _isWaitedAttack;
        private float _speedAfterAttack;

        private PlayerTankWrapper _playerTankWrapper;
        private Aiming _aiming;

        private bool _isPlayerAttacked;

        protected override float Speed => _isPlayerAttacked ? _speedAfterAttack : base.Speed;

        private void OnDestroy() =>
            _aiming.Shooted -= OnPlayerShooted;

        public void Initialize(float speedAfterAttack, bool isWaitedAttack, bool isPathLooped)
        {
            _speedAfterAttack = speedAfterAttack;
            _isPatLooped = isPathLooped;
            _isWaitedAttack = isWaitedAttack;

            Enemy enemy = GetComponent<Enemy>();

            _playerTankWrapper = enemy.PlayerTankWrapper;
            _aiming = enemy.Aiming;

            _isPlayerAttacked = false;

            if (_isWaitedAttack == false)
                StartMovement();

            _aiming.Shooted += OnPlayerShooted;
        }

        private void OnPlayerShooted()
        {
            _isPlayerAttacked = true;

            if(_isWaitedAttack)
                StartMovement();
        }

        protected override bool CanMoveNextCircle() =>
            _isPatLooped;
    }
}