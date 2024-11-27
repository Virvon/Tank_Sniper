using Assets.Sources.Gameplay.Enemies.Animation;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.Enemies.StateMachine.States
{
    public class EnemyAttackState : IState
    {
        private readonly Enemy _enemy;
        private readonly PlayerTank _playerTank;

        public EnemyAttackState(Enemy enemy, PlayerTank playerTank)
        {
            _enemy = enemy;
            _playerTank = playerTank;
        }

        public UniTask Enter()
        {
            _enemy.Animator.SetBool(AnimationPath.IsShooted, true);
            _enemy.Rotate(_playerTank, callback: () => _enemy.StartShooting());

            return default;
        }

        public UniTask Exit()
        {
            return default;
        }

        //private IEnumerator Shooter()
        //{
        //    WaitForSeconds duration = new(_shootingCoolDown);
        //    _isShooted = true;

        //    while (_isShooted)
        //    {
        //        _enemy.StartShoot((_playerTank.transform.position - _enemy.ShootPoint.position).normalized);

        //        yield return duration;
        //    }     
        //}
    }
}
