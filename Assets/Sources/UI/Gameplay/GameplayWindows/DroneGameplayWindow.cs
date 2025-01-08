using Assets.Sources.Gameplay.Player.Aiming;
using Assets.Sources.Gameplay.Player.Wrappers;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Gameplay.GameplayWindows
{
    public class DroneGameplayWindow : GameplayWindow
    {
        [SerializeField] private CanvasGroup _gameplayInfoCanvasGroup;

        private PlayerDroneWrapper _playerDroneWrapper;
        private DroneAiming _droneAiming;

        [Inject]
        private void Construct(PlayerDroneWrapper playerDroneWrapper, DroneAiming droneAiming)
        {
            _playerDroneWrapper = playerDroneWrapper;
            _droneAiming = droneAiming;

            _playerDroneWrapper.DroneExploided += OnDroneExploided;
            _droneAiming.Shooted += OnShooted;
        }

        protected override void OnDestroy()
        {
            _playerDroneWrapper.DroneExploided -= OnDroneExploided;
            _droneAiming.Shooted -= OnShooted;
        }

        private void OnShooted()
        {
            SetAimButtonActive(false);
            OverviewAimCanvasGroup.alpha = 0;
            AimingCanvasGroup.alpha = 1;
            _gameplayInfoCanvasGroup.alpha = 0;
        }

        private void OnDroneExploided()
        {
            SetAimButtonActive(true);
            OverviewAimCanvasGroup.alpha = 1;
            AimingCanvasGroup.alpha = 0;
            _gameplayInfoCanvasGroup.alpha = 1;
        }
    }
}