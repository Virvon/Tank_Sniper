using Assets.Sources.Infrastructure.Factories.MainMenuFactory;
using UnityEngine;
using Zenject;

namespace Assets.Sources.MainMenu
{
    public class DeskCell : MonoBehaviour
    {
        [SerializeField] private Transform _tankPoint;

        private Tank _tank;

        public bool IsEmpty => _tank == null;

        public void PutTank(Tank tank)
        {
            _tank = tank;
            _tank.transform.position = _tankPoint.position;
            _tank.transform.parent = transform;
        }

        public Tank GetTank()
        {
            Tank tank = _tank;
            _tank = null;

            return tank;
        }
    }
}
