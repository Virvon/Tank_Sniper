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

        [Inject]
        private void Construct(PlayerTankWrapper playerTankWrapper, Aiming aiming)
        {
            _playerTankWrapper = playerTankWrapper;
            _aiming = aiming;

            _isAttacked = false;

            _aiming.Shooted += OnShooted;
        }

        private void OnDestroy() =>
            _aiming.Shooted -= OnShooted;

        public void Initialize(float speedAfterAttack)
        {
            _speedAfterAttack = speedAfterAttack;
            _isLooped = true;
        }

        private void OnShooted()
        {
            _isAttacked = true;
        }

        protected override void Start()
        {
            base.Start();
            StartMovement();
        }

        protected override bool CanMoveNextCircle() =>
            _isLooped;
    }
}