using Assets.Sources.Infrastructure.Factories.MainMenuFactory;
using Assets.Sources.Services.PersistentProgress;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Sources.MainMenu
{
    public class DeskCell : MonoBehaviour
    {
        [SerializeField] private Transform _tankPoint;
        [SerializeField] private Marker _marker;
        [SerializeField] private Quaternion _tankRotation;
        [SerializeField] private float _tankScale;

        private IMainMenuFactory _mainMenuFactory;
        private IPersistentProgressService _persistentProgressService;

        private Tank _tank;

        public bool IsEmpty => _tank == null;

        public event Action EmploymentChanged;

        [Inject]
        private void Construct(IMainMenuFactory mainMenuFactory, IPersistentProgressService persistentProgressService)
        {
            _mainMenuFactory = mainMenuFactory;
            _persistentProgressService = persistentProgressService;
        }

        public async UniTask CreateTank(uint level)
        {
            _tank = await _mainMenuFactory.CreateTank(level, _tankPoint.position, _tankRotation, transform);
            _tank.transform.localScale = Vector3.one * _tankScale;

            EmploymentChanged?.Invoke();
        }

        public async UniTask UpgradeTank()
        {
            uint targetLevel = _tank.Level + 1;

            _persistentProgressService.Progress.TryUnlockTank(targetLevel);

            _tank.Destroy();

            await CreateTank(targetLevel);
        }

        public void PlaceTank(Tank tank)
        {
            _tank = tank;
            _tank.transform.position = _tankPoint.position;
            _tank.transform.parent = transform;

            EmploymentChanged?.Invoke();
        }

        public Tank GetTank()
        {
            Tank tank = _tank;
            _tank = null;

            return tank;
        }

        public bool CanPlace(uint tankLevel) =>
            (IsEmpty == false && _tank.Level != tankLevel) == false;

        public void Mark(uint tankLevel) =>
            _marker.Mark(CanPlace(tankLevel));

        public void HideMark() =>
            _marker.Hide();
    }
}
