using Assets.Sources.Infrastructure.Factories.MainMenuFactory;
using Assets.Sources.Services.PersistentProgress;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Assets.Sources.MainMenu
{
    public class Desk : MonoBehaviour
    {
        [SerializeField] private DeskCell[] _cells;
        [SerializeField] private Transform _selectedTankPoint;
        [SerializeField] private Quaternion _selectedTankRotation;

        private IPersistentProgressService _persistentProgressService;
        private IMainMenuFactory _mainMenuFactory;

        private Tank _selectedTank;

        public event Action<bool> EmploymentChanged;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, IMainMenuFactory mainMenuFactory)
        {
            _persistentProgressService = persistentProgressService;
            _mainMenuFactory = mainMenuFactory;

            foreach (DeskCell deskCell in _cells)
                deskCell.EmploymentChanged += OnDeskCellEmploymentChanged;

            _persistentProgressService.Progress.SelectedTankChanged += ChangeSelectedTank;

            OnDeskCellEmploymentChanged();
            ChangeSelectedTank(_persistentProgressService.Progress.SelectedTankLevel);
        }

        private void OnDestroy()
        {
            foreach (DeskCell deskCell in _cells)
                deskCell.EmploymentChanged -= OnDeskCellEmploymentChanged;

            _persistentProgressService.Progress.SelectedTankChanged -= ChangeSelectedTank;
        }

        public async UniTask CreateTank()
        {
            DeskCell[] emptyCells = _cells.Where(cell => cell.IsEmpty).ToArray();

            DeskCell cell = emptyCells[Random.Range(0, emptyCells.Length)];

            await cell.CreateTank(1);
        }

        private void OnDeskCellEmploymentChanged() =>
            EmploymentChanged?.Invoke(_cells.Any(cell => cell.IsEmpty));

        private async void ChangeSelectedTank(uint level)
        {
            _selectedTank?.Destroy();
            _selectedTank = await _mainMenuFactory.CreateTank(level, _selectedTankPoint.position, _selectedTankRotation, transform);
        }

        public class Factory : PlaceholderFactory<string, UniTask<Desk>>
        {
        }
    }
}
