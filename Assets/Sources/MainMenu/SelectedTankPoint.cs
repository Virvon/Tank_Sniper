using Assets.Sources.Data;
using Assets.Sources.Infrastructure.Factories.MainMenuFactory;
using Assets.Sources.Infrastructure.Factories.TankFactory;
using Assets.Sources.Services.PersistentProgress;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.MainMenu
{
    public abstract class SelectedTankPoint : MonoBehaviour
    {
        [SerializeField] private Transform _tankPoint;
        [SerializeField] private Quaternion _spawnRotation;
        [SerializeField] private TankScalingAnimator _scalingAnimator;

        private IPersistentProgressService _persistentProgressService;
        private ITankFactory _tankFactory;

        protected GameObject SelectedTank { get; private set; }
        protected Transform TankPoint => _tankPoint;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, ITankFactory tankFactory)
        {
            _persistentProgressService = persistentProgressService;
            _tankFactory = tankFactory;

            _persistentProgressService.Progress.SelectedTankChanged += OnSelectedTankChanged;          
        }

        private async void Start() =>
            await OnStart();

        private void OnDestroy() =>
            _persistentProgressService.Progress.SelectedTankChanged -= OnSelectedTankChanged;

        private async void OnSelectedTankChanged(uint level) =>
            await ChangeSelectedTank(level, true);

        protected virtual async UniTask ChangeSelectedTank(uint level, bool needToAnimate)
        {
            TankData tankData = _persistentProgressService.Progress.GetTank(level);

            if (SelectedTank != null)
                Destroy(SelectedTank);

            SelectedTank = await CreateTank(tankData, _tankPoint.position, GetRotation(), GetParent(), _tankFactory);

            if (needToAnimate)
                _scalingAnimator.Play();
        }

        protected abstract UniTask<GameObject> CreateTank(
            TankData tankData,
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            ITankFactory tankFactory);

        protected virtual Quaternion GetRotation() =>
            _spawnRotation;

        protected async virtual UniTask OnStart() =>
            await ChangeSelectedTank(_persistentProgressService.Progress.SelectedTankLevel, false);

        protected virtual Transform GetParent() =>
            transform;
    }
}