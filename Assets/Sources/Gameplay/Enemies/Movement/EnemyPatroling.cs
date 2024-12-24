using Assets.Sources.Gameplay.Enemies.Animation;
using Assets.Sources.Gameplay.Player;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies.Movement
{
    public class EnemyPatroling : EnemyMovement
    {
        private const int RayCastDistance = 300;

        [SerializeField] private float _stoppingDuration;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Animator _animator;

        private PlayerTankWrapper _playerTankWrapper;
        private Aiming _aiming;

        private bool _isStartedStopping;

        public bool CanShoot { get; private set; }

        protected override float StoppingDuration => _stoppingDuration;

        [Inject]
        private void Construct(PlayerTankWrapper playerTankWrapper, Aiming aiming)
        {
            _playerTankWrapper = playerTankWrapper;
            _aiming = aiming;

            _isStartedStopping = false;
            CanShoot = false;

            _aiming.Shooted += OnPlayerShooted;
            PointFinished += OnPointFinished;
            NextPointStarted += OnNextPointStarted;
        }

        private void Start() =>
            StartMovement();

        private void OnDestroy()
        {
            _aiming.Shooted -= OnPlayerShooted;
            PointFinished -= OnPointFinished;
            NextPointStarted -= OnNextPointStarted;
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

            StartCoroutine(Stopper());
        }

        private bool CheckPlayerTankVisibility()
        {
            return Physics.Raycast(transform.position, (_playerTankWrapper.transform.position - transform.position).normalized, out RaycastHit hitInfo, RayCastDistance, _layerMask)
                && hitInfo.transform.TryGetComponent(out PlayerTankWrapper _);
        }

        private IEnumerator Stopper()
        {
            yield return new WaitWhile(() => CheckPlayerTankVisibility() == false);

            StopMovement();
            CanShoot = true;
        }
    }
}
