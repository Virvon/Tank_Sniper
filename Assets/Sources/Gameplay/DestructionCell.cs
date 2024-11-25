using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Sources.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class DestructionCell : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private bool _needToFindNeighboringCells = true;
        [SerializeField] private List<DestructionCell> _neighboringCells;

        [SerializeField] private bool _isFoundation = false;

        private Rigidbody _rigidbody;

        public bool IsBreaked { get; private set; }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();

            IsBreaked = false;
        }

        private void OnValidate()
        {
            if (_needToFindNeighboringCells == false)
                return;

            _neighboringCells = new List<DestructionCell>();

            DestructionCell[] destructionCells = FindObjectsOfType<DestructionCell>();

            destructionCells = destructionCells.OrderBy(cell => Vector3.Distance(cell.transform.position, transform.position)).ToArray();

            for (int i = 0; i < destructionCells.Length && i < 4; i++)
                _neighboringCells.Add(destructionCells[i]);
        }

        private void OnDrawGizmos()
        {
            if (_neighboringCells == null)
                return;

            if (_isFoundation)
                Gizmos.color = Color.blue;
            else if (IsConnectedToFondation(new()))
                Gizmos.color = Color.yellow;
            else
                Gizmos.color = Color.red;

            Gizmos.DrawSphere(transform.position, 0.2f);

            foreach (DestructionCell destructionCell in _neighboringCells)
            {
                if (destructionCell != null)
                    Gizmos.DrawLine(transform.position, destructionCell.transform.position);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //Breake();

            //Debug.Log(_neighboringCells[0].IsConnectedToFondation(new()));
        }

        public void ReportDestruction(List<DestructionCell> checkedCells)
        {
            if (IsConnectedToFondation(checkedCells) == false)
            {
                //Debug.Log(name + " need to breake");

                Breake();
            }
        }

        public bool IsConnectedToFondation(List<DestructionCell> checkedCells)
        {
            if (_isFoundation)
                return true;

            checkedCells.Add(this);

            foreach (DestructionCell destructionCell in _neighboringCells)
            {
                if (destructionCell == null)
                    _neighboringCells.Remove(destructionCell);
                else if (checkedCells.Contains(destructionCell) || destructionCell.IsBreaked)
                    continue;
                else if (destructionCell.IsConnectedToFondation(checkedCells))
                    return true;
            }

            return false;
        }

        private void Breake()
        {
            if (IsBreaked)
                return;

            IsBreaked = true;

            List<DestructionCell> checkedCells = new();

            foreach (DestructionCell neigboringCell in _neighboringCells)
                neigboringCell.ReportDestruction(checkedCells);

            _rigidbody.isKinematic = false;

            //Destroy(gameObject, 2);
        }
    }
}