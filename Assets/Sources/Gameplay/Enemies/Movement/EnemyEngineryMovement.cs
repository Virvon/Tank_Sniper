using Assets.Sources.Gameplay.Player;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies.Movement
{
    public class EnemyEngineryMovement : EnemyMovement
    {
        private bool _isPatLooped;
        private bool _isWaitedAttack;
        private float _speedAfterAttack;
        private float _stoppingDuration;

        private Aiming _aiming;

        private bool _isPlayerAttacked;

        public bool IsWaitedAttack => _isWaitedAttack;
        protected override float Speed => _isPlayerAttacked ? _speedAfterAttack : base.Speed;
        protected override float StoppingDuration => _stoppingDuration;

        private void OnDestroy() =>
            _aiming.Shooted -= OnPlayerShooted;

        public void Initialize(float speedAfterAttack, bool isWaitedAttack, bool isPathLooped, float stoppingDuration)
        {
            _speedAfterAttack = speedAfterAttack;
            _isPatLooped = isPathLooped;
            _isWaitedAttack = isWaitedAttack;
            _stoppingDuration = stoppingDuration;

            Enemy enemy = GetComponent<Enemy>();

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