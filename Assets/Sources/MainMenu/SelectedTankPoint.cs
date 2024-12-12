using Assets.Sources.Data;
using Assets.Sources.Infrastructure.Factories.MainMenuFactory;
using Assets.Sources.Services.PersistentProgress;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.MainMenu
{
    public class SelectedTankPoint : MonoBehaviour
    {
        [SerializeField] private Transform _tankPoint;
        [SerializeField] private Quaternion _spawnRotation;

        private IPersistentProgressService _persistentProgressService;
        private IMainMenuFactory _mainMenuFactory;

        protected Tank SelectedTank { get; private set; }
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

            SelectedTank?.Destroy();
            SelectedTank = await _mainMenuFactory.CreateTank(level, _tankPoint.position, GetRotation(), GetParent(), tankData.SkinType);
        }

        protected virtual Quaternion GetRotation() =>
            _spawnRotation;

        protected async virtual UniTask OnStart() =>
            await ChangeSelectedTank(_persistentProgressService.Progress.SelectedTankLevel);

        protected virtual Transform GetParent() =>
            transform;
    }
}
