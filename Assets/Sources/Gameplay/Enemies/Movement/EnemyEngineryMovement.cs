using Assets.Sources.Gameplay.Player;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies.Movement
{
    public class EnemyEngineryMovement : EnemyMovement
    {
        private bool _isLooped;
        private bool _isWaitedAttack;
        private float _speedAfterAttack;

        private PlayerTankWrapper _playerTankWrapper;
        private Aiming _aiming;

        private bool _isAttacked;

        protected override float Speed => _isAttacked ? _speedAfterAttack : base.Speed;

        private void OnDestroy() =>
            _aiming.Shooted -= OnShooted;

        public void Initialize(float speedAfterAttack)
        {
            _speedAfterAttack = speedAfterAttack;
            _isLooped = true;

            Enemy enemy = GetComponent<Enemy>();

            _playerTankWrapper = enemy.PlayerTankWrapper;
            _aiming = enemy.Aiming;

            _isAttacked = false;

            _aiming.Shooted += OnShooted;

            StartMovement();
        }

        private void OnShooted()
        {
            _isAttacked = true;
        }

        protected override bool CanMoveNextCircle() =>
            _isLooped;
    }
}