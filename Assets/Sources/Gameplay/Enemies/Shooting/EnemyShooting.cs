using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Gameplay.Handlers;
using Assets.Sources.Gameplay.Player.Aiming;
using Assets.Sources.Gameplay.Player.Wrappers;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Weapons
{
    public abstract class EnemyShooting : MonoBehaviour
    {
        private readonly Vector3 TargetOffset = new Vector3(0, 2, 0);

        [SerializeField] private Enemy _enemy;

        private GameplaySettingsConfig _gameplaySettings;
        private IShootedAiming _aiming;
        private DefeatHandler _defeatHandler;

        private bool _isShootingStarted;
        private bool _isPlayerDefeated;

        public PlayerWrapper PlayerWrapper { get; private set; }

        protected bool IsShooted { get; private set; }
        protected virtual bool CanShoot => _isPlayerDefeated == false;

        [Inject]
        private void Construct(
            IStaticDataService staticDataService,
            PlayerWrapper playerWrapper,
            IShootedAiming aiming,
            DefeatHandler defeatHandler)
        {
            _gameplaySettings = staticDataService.GameplaySettingsConfig;
            PlayerWrapper = playerWrapper;
            _aiming = aiming;
            _defeatHandler = defeatHandler;

            _isShootingStarted = false;
            _isPlayerDefeated = false;
            IsShooted = false;

            _aiming.Shooted += OnPlayerTankAttacked;
            _defeatHandler.Defeated += OnPlayerDefeated;
            _defeatHandler.ProgressRecovered += OnProgressRecovery;
            _enemy.Destructed += OnEnemyDestructed;
        }

        private void OnDestroy()
        {
            _aiming.Shooted -= OnPlayerTankAttacked;
            _defeatHandler.Defeated -= OnPlayerDefeated;
            _defeatHandler.ProgressRecovered -= OnProgressRecovery;
            _enemy.Destructed -= OnEnemyDestructed;
        }

        protected Quaternion GetShootingRotation()
        {
            Vector2 randomOffset = Random.insideUnitCircle * _gameplaySettings.EnemyScatter;
            Vector3 targetPosition = PlayerWrapper.transform.position + TargetOffset + new Vector3(randomOffset.x, randomOffset.y, 0);

            return Quaternion.LookRotation((targetPosition - GetCurrentShootingPosition()).normalized);
        }

        protected abstract Vector3 GetCurrentShootingPosition();

        protected virtual void StartShooting() =>
            StartCoroutine(Shooter());

        protected virtual void OnEnemyDestructed() =>
            IsShooted = false;

        private void OnPlayerTankAttacked()
        {
            if (_isShootingStarted)
                return;

            _isShootingStarted = true;
            IsShooted = true;

            StartShooting();
        }

        private void OnPlayerDefeated() =>
            _isPlayerDefeated = true;

        private void OnProgressRecovery() =>
            _isPlayerDefeated = false;

        protected abstract IEnumerator Shooter();
    }
}