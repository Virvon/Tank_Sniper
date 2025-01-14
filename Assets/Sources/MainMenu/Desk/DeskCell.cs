using Assets.Sources.Infrastructure.Factories.MainMenuFactory;
using Assets.Sources.Infrastructure.Factories.TankFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Tanks;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Sources.MainMenu.Desk
{
    public class DeskCell : MonoBehaviour
    {
        [SerializeField] private Transform _tankPoint;
        [SerializeField] private Marker _marker;
        [SerializeField] private Quaternion _tankRotation;
        [SerializeField] private float _tankScale;

        private ITankFactory _tankFactory;
        private IPersistentProgressService _persistentProgressService;
        private IStaticDataService _staicDataService;

        private Tank _tank;

        public bool IsEmpty => _tank == null;

        public event Action EmploymentChanged;

        [Inject]
        private void Construct(ITankFactory tankFactory, IPersistentProgressService persistentProgressService, IStaticDataService staticDataService)
        {
            _tankFactory = tankFactory;
            _persistentProgressService = persistentProgressService;
            _staicDataService = staticDataService;
        }

        public async UniTask CreateTank(uint level)
        {
            _tank = await _tankFactory.CreateTank(level, _tankPoint.position, _tankRotation, transform);
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

        public bool CanPlace(uint tankLevel)
        {
            if (IsEmpty == false && _staicDataService.TankConfigs.Any(config => config.Level == _tank.Level + 1) == false)
                return false;

            return (IsEmpty == false && _tank.Level != tankLevel) == false;
        }

        public void Mark(uint tankLevel) =>
            _marker.Mark(CanPlace(tankLevel));

        public void HideMark() =>
            _marker.Hide();
    }
}
