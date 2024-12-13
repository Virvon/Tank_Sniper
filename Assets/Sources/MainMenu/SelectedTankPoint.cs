using Assets.Sources.Data;
using Assets.Sources.Infrastructure.Factories.MainMenuFactory;
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

        private IPersistentProgressService _persistentProgressService;
        private IMainMenuFactory _mainMenuFactory;

        protected GameObject SelectedTank { get; private set; }
        protected Transform TankPoint => _tankPoint;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, IMainMenuFactory mainMenuFactory)
        {
            _persistentProgressService = persistentProgressService;
            _mainMenuFactory = mainMenuFactory;

            _persistentProgressService.Progress.SelectedTankChanged += OnSelectedTankChanged;          
        }

        private async void Start() =>
            await OnStart();

        private void OnDestroy() =>
            _persistentProgressService.Progress.SelectedTankChanged -= OnSelectedTankChanged;

        private async void OnSelectedTankChanged(uint level) =>
            await ChangeSelectedTank(level);

        protected virtual async UniTask ChangeSelectedTank(uint level)
        {
            TankData tankData = _persistentProgressService.Progress.GetTank(level);

            if (SelectedTank != null)
                Destroy(SelectedTank);

            SelectedTank = await CreateTank(tankData, _tankPoint.position, GetRotation(), GetParent(), _mainMenuFactory);
        }

        protected abstract UniTask<GameObject> CreateTank(
            TankData tankData,
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            IMainMenuFactory mainMenuFactory);

        protected virtual Quaternion GetRotation() =>
            _spawnRotation;

        protected async virtual UniTask OnStart() =>
            await ChangeSelectedTank(_persistentProgressService.Progress.SelectedTankLevel);

        protected virtual Transform GetParent() =>
            transform;
    }
}