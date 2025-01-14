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

        public event Action<bool> EmploymentChanged;

        public bool HasEmptyCells => _cells.Any(cell => cell.IsEmpty);

        private void Start()
        {
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
        }

        private void OnDeskCellEmploymentChanged() =>
            EmploymentChanged?.Invoke(HasEmptyCells);



        public class Factory : PlaceholderFactory<string, UniTask<Desk>>
        {
        }
    }
}
