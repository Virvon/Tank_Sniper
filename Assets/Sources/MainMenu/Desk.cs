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

        public event Action<bool> EmploymentChanged;

        private void Start()
        {
            OnDeskCellEmploymentChanged();

            foreach (DeskCell deskCell in _cells)
                deskCell.EmploymentChanged += OnDeskCellEmploymentChanged;
        }

        private void OnDestroy()
        {
            foreach (DeskCell deskCell in _cells)
                deskCell.EmploymentChanged -= OnDeskCellEmploymentChanged;
        }

        public async UniTask CreateTank()
        {
            DeskCell[] emptyCells = _cells.Where(cell => cell.IsEmpty).ToArray();

            DeskCell cell = emptyCells[Random.Range(0, emptyCells.Length)];

            await cell.CreateTank(1);
        }

        private void OnDeskCellEmploymentChanged() =>
            EmploymentChanged?.Invoke(_cells.Any(cell => cell.IsEmpty));

        public class Factory : PlaceholderFactory<string, UniTask<Desk>>
        {
        }
    }
}
