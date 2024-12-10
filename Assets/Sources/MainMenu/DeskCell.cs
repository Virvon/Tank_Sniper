using Assets.Sources.Infrastructure.Factories.MainMenuFactory;
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

        private Tank _tank;

        public bool IsEmpty => _tank == null;

        public event Action EmploymentChanged;

        [Inject]
        private void Construct(IMainMenuFactory mainMenuFactory)
        {
            _mainMenuFactory = mainMenuFactory;
        }

        public async UniTask CreateTank(uint level)
        {
            Tank tank = await _mainMenuFactory.CreateTank(level, _tankRotation);
            tank.transform.localScale = Vector3.one * _tankScale;

            PlaceTank(tank);
        }

        public async UniTask UpgradeTank()
        {
            uint targetLevel = _tank.Level + 1;

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
