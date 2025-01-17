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
        [SerializeField] private string _id;

        private ITankFactory _tankFactory;
        private IPersistentProgressService _persistentProgressService;
        private IStaticDataService _staicDataService;

        private DeskTankWrapper _tankWrapper;

        public bool IsEmpty => _tankWrapper == null;

        public event Action EmploymentChanged;

        [Inject]
        private async void Construct(
            ITankFactory tankFactory,
            IPersistentProgressService persistentProgressService,
            IStaticDataService staticDataService)
        {
            _tankFactory = tankFactory;
            _persistentProgressService = persistentProgressService;
            _staicDataService = staticDataService;

            await Initialize();
        }

        public async UniTask CreateTank(uint level, bool needAnimate)
        {
            _tankWrapper = await _tankFactory.CreateDeskTankWrapper(_tankPoint.position, transform);
            _tankWrapper.Initialize(level);

            Tank tank = await _tankFactory.CreateTank(level, _tankWrapper.TankPoint.position, _tankRotation, _tankWrapper.TankPoint, string.Empty, string.Empty);
            tank.transform.localScale = Vector3.one * _tankScale;

            if (needAnimate)
                _tankWrapper.Animate();

            _persistentProgressService.Progress.DeskData.UpdateCellInfo(_id, tank.Level);

            EmploymentChanged?.Invoke();
        }

        public async UniTask UpgradeTank()
        {
            uint targetLevel = _tankWrapper.TankLevel + 1;

            _persistentProgressService.Progress.TryUnlockTank(targetLevel);

            _tankWrapper.Destroy();

            await CreateTank(targetLevel, true);
        }

        public void PlaceTank(DeskTankWrapper tankWrapper)
        {
            _tankWrapper = tankWrapper;
            _tankWrapper.transform.position = _tankPoint.position;
            _tankWrapper.transform.parent = transform;
            _persistentProgressService.Progress.DeskData.UpdateCellInfo(_id, _tankWrapper.TankLevel);

            EmploymentChanged?.Invoke();
        }

        public DeskTankWrapper GetTankWrapper()
        {
            DeskTankWrapper tankWrapper = _tankWrapper;
            _tankWrapper = null;
            _persistentProgressService.Progress.DeskData.UpdateCellInfo(_id, 0);

            return tankWrapper;
        }

        public bool CanPlace(uint tankLevel)
        {
            if (IsEmpty == false && _staicDataService.TankConfigs.Any(config => config.Level == _tankWrapper.TankLevel + 1) == false)
                return false;

            return (IsEmpty == false && _tankWrapper.TankLevel != tankLevel) == false;
        }

        public void Mark(uint tankLevel) =>
            _marker.Mark(CanPlace(tankLevel));

        public void HideMark() =>
            _marker.Hide();

        private async UniTask Initialize()
        {
            uint tankLevel = _persistentProgressService.Progress.DeskData.GetCellInfo(_id);

            if(tankLevel != 0)
                await CreateTank(tankLevel, false);
        }
    }
}
