using Assets.Sources.Services.SaveLoadProgress;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Assets.Sources.MainMenu.Desk
{
    public class Desk : MonoBehaviour
    {
        [SerializeField] private DeskCell[] _cells;

        private ISaveLoadService _saveLoadService;

        public event Action<bool> EmploymentChanged;

        public bool HasEmptyCells => _cells.Any(cell => cell.IsEmpty);

        [Inject]
        private void Construct(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;

            foreach (DeskCell deskCell in _cells)
                deskCell.EmploymentChanged += OnDeskCellEmploymentChanged;

            OnDeskCellEmploymentChanged();
        }

        private void OnDestroy()
        {
            foreach (DeskCell deskCell in _cells)
                deskCell.EmploymentChanged -= OnDeskCellEmploymentChanged;
        }

        public async UniTask CreateTank(uint level)
        {
            DeskCell[] emptyCells = _cells.Where(cell => cell.IsEmpty).ToArray();

            DeskCell cell = emptyCells[Random.Range(0, emptyCells.Length)];

            await cell.CreateTank(level);

            _saveLoadService.SaveProgress();
        }

        private void OnDeskCellEmploymentChanged() =>
            EmploymentChanged?.Invoke(HasEmptyCells);

        public class Factory : PlaceholderFactory<string, UniTask<Desk>>
        {
        }
    }
}
