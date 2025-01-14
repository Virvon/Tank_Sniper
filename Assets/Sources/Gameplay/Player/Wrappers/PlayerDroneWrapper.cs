using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.Handlers;
using Assets.Sources.Gameplay.Player.Aiming;
using Assets.Sources.Gameplay.Player.Weapons;
using Assets.Sources.Infrastructure.Factories.TankFactory;
using Assets.Sources.Services.PersistentProgress;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Player.Wrappers
{
    public class PlayerDroneWrapper : PlayerWrapper, IShootable
    {
        [SerializeField] private float _dronRotationSpeed;
        [SerializeField] private float _playerCharacterRotation;

        private AimingCameraPoint _aimingCameraPoint;
        private ITankFactory _tankFactory;
        private RotationCamera _rotationCamera;
        private DroneAiming _aiming;
        private DefeatHandler _defeatHandler;
        private WictoryHandler _wictoryHangler;

        private uint _maxDronesCount;
        private uint _dronesCount;

        private Drone _drone;
        private Coroutine _rotatiter;
        private bool _isDronAimed;

        public uint BulletsCount => _dronesCount;

        public event Action DroneExploided;
        public event Action BulletsCountChanged;

        [Inject]
        private void Construct(
            AimingCameraPoint aimingCameraPoint,
            ITankFactory tankFactory,
            RotationCamera rotationCamera,
            DroneAiming droneAiming,
            DefeatHandler defeatHandler,
            uint dronesCount,
            WictoryHandler wictoryHandler,
            IPersistentProgressService persistentProgressService)
        {
            _aimingCameraPoint = aimingCameraPoint;
            _tankFactory = tankFactory;
            _rotationCamera = rotationCamera;
            _aiming = droneAiming;
            _defeatHandler = defeatHandler;
            _maxDronesCount = dronesCount;
            _wictoryHangler = wictoryHandler;

            _dronesCount = _maxDronesCount;

            tankFactory.CreatePlayerCharacter(
                persistentProgressService.Progress.SelectedPlayerCharacter,
                transform.position,
                Quaternion.Euler(0, transform.rotation.eulerAngles.y + _playerCharacterRotation, 0),
                transform);

            _aiming.Shooted += OnPlayerShooted;
            _defeatHandler.ProgressRecovered += OnProgressRecovered;
        }

        private async void Start() =>
            await CreateDrone();

        private void OnDestroy()
        {
            _aiming.Shooted -= OnPlayerShooted;
            _defeatHandler.ProgressRecovered -= OnProgressRecovered;

            if (_drone != null)
                _drone.Exploded -= OnDroneExploded;
        }

        private async void OnProgressRecovered()
        {
            _dronesCount = _maxDronesCount;

            await CreateDrone();
        }

        private void OnPlayerShooted() =>
            _isDronAimed = false;

        private async UniTask CreateDrone()
        {
            _drone = await _tankFactory.CreateDrone(
                            _aimingCameraPoint.transform.position,
                            Quaternion.Euler(0, _rotationCamera.transform.rotation.eulerAngles.y, 0));

            _drone.Exploded += OnDroneExploded;

            _isDronAimed = true;

            if (_rotatiter != null)
                StopCoroutine(_rotatiter);

            _rotatiter = StartCoroutine(Rotater());
        }

        private async void OnDroneExploded()
        {
            DroneExploided?.Invoke();
            _drone.Exploded -= OnDroneExploded;

            if(_wictoryHangler.IsWoned)
            {
                return;
            }
            else if (_dronesCount == 0)
            {
                _defeatHandler.OnDefeat();

                return;
            }

            _dronesCount--;
            
            await CreateDrone();

            BulletsCountChanged?.Invoke();
        }

        private IEnumerator Rotater()
        {
            while (_isDronAimed)
            {
                Vector3 targetDiretion = new Vector3(_rotationCamera.transform.forward.x, 0, _rotationCamera.transform.forward.z);
                Quaternion targetRotation = Quaternion.LookRotation(targetDiretion);
                _drone.transform.rotation = Quaternion.RotateTowards(_drone.transform.rotation, targetRotation, _dronRotationSpeed * Time.deltaTime);

                yield return null;
            }
        }

        public class Factory : PlaceholderFactory<string, Vector3, Quaternion, UniTask<PlayerWrapper>>
        {
        }
    }
}
