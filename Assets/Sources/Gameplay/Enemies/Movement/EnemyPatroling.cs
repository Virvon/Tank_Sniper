using Assets.Sources.Gameplay.Enemies.Animation;
using Assets.Sources.Gameplay.Player.Aiming;
using Assets.Sources.Gameplay.Weapons;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Movement
{
    public class EnemyPatroling : EnemyMovement
    {
        private float _stoppingDuration;
        private Animator _animator;
        private PatrolingEnemyShooting _enemyShooting;

        private IShootedAiming _aiming;

        private bool _isStartedStopping;

        public bool CanShoot { get; private set; }

        protected override float StoppingDuration => _stoppingDuration;

        private void OnDestroy()
        {
            _aiming.Shooted -= OnPlayerShooted;
            PointFinished -= OnPointFinished;
            NextPointStarted -= OnNextPointStarted;
        }

        public void Initialize(float stoppingDuration)
        {
            _stoppingDuration = stoppingDuration;

            _animator = GetComponentInChildren<Animator>();
            _enemyShooting = GetComponent<PatrolingEnemyShooting>();
            Enemy enemy = GetComponent<Enemy>();

            _aiming = enemy.Aiming;

            _isStartedStopping = false;
            CanShoot = false;

            _aiming.Shooted += OnPlayerShooted;
            PointFinished += OnPointFinished;
            NextPointStarted += OnNextPointStarted;

            StartMovement();
        }

        private void OnNextPointStarted()
        {
            _animator.SetBool(AnimationPath.IsWalked, true);
            CanShoot = false;
        }

        private void OnPointFinished()
        {
            _animator.SetBool(AnimationPath.IsWalked, false);
            CanShoot = true;
        }

        private void OnPlayerShooted()
        {
            if (_isStartedStopping)
                return;

            _isStartedStopping = true;

            StartCoroutine(Stopper());
        }

        private IEnumerator Stopper()
        {
            yield return new WaitWhile(() => _enemyShooting.CheckPlayerTankVisibility() == false);

            StopMovement();
            _enemyShooting.CanShooting();
        }
    }
}
