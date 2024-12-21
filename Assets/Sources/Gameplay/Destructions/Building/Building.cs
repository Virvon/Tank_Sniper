using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.Gameplay.Destructions.Building
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private DestructionCell[] _destructionCells;
        [SerializeField] private int _connectsCount;
        [SerializeField] private float _maxDistance;

        private void Start() =>
            Collect();

        public void Collect()
        {
            foreach (var cell in _destructionCells)
                cell.Clear();

            foreach (DestructionCell cell in _destructionCells)
            {
                int conectionsCound = _connectsCount;
                List<DestructionCell> cells = _destructionCells.Where(value => Vector3.Distance(cell.transform.position, value.transform.position) < _maxDistance && value != cell).ToList();

                for (int i = 0; i < conectionsCound; i++)
                {
                    var foundationConnectedCell = cells.Where(value => value.Contains(cell) == false && value.IsConnectedToFondation(new())).FirstOrDefault();

                    if (foundationConnectedCell != null)
                    {
                        foundationConnectedCell.AddNeighboring(cell);
                        cell.AddNeighboring(foundationConnectedCell);
                    }
                    else
                    {
                        var connectedCell = cells[Random.Range(0, cells.Count)];

                        connectedCell.AddNeighboring(cell);
                        cell.AddNeighboring(connectedCell);
                    }
                }
            }
        }
    }
}