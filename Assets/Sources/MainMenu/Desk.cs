using Assets.Sources.Infrastructure.Factories.MainMenuFactory;
using Assets.Sources.UI;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Sources.MainMenu
{
    public class Desk : MonoBehaviour
    {
        [SerializeField] private DeskCell[] _cells;
        [SerializeField] private float _tankScale;
        [SerializeField] private Quaternion _tankRotation;

        private MainMenuWindow _mainMenuWindow;
        private IMainMenuFactory _mainMenuFactory;

        [Inject]
        private void Construct(MainMenuWindow mainMenuWindow, IMainMenuFactory mainMenuFactory)
        {
            _mainMenuWindow = mainMenuWindow;
            _mainMenuFactory = mainMenuFactory;

            _mainMenuWindow.BuyTankButtonClicked += OnBuyTankButtonClicked;
        }

        private void OnDestroy() =>
            _mainMenuWindow.BuyTankButtonClicked -= OnBuyTankButtonClicked;

        private async void OnBuyTankButtonClicked()
        {
            DeskCell[] emptyCells = _cells.Where(cell => cell.IsEmpty).ToArray();

            DeskCell cell = emptyCells[Random.Range(0, emptyCells.Length)];

            Tank tank = await _mainMenuFactory.CreateTank(Vector3.zero, _tankRotation, transform);
            tank.transform.localScale = Vector3.one * _tankScale;

            cell.PutTank(tank);
        }

        public class Factory : PlaceholderFactory<string, UniTask<Desk>>
        {
        }
    }
}
