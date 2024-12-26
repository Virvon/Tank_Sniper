using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Gameplay.Destructions.Building
{
    [RequireComponent(typeof(Rigidbody))]
    public class DestructionCell : DestructionPart, IDamageable
    {
        private const float ScalingSpeed = 1;
        private const float DestroyDelay = 2;
        private const string DestroedLayer = "IgnoreProjectile";

        [SerializeField] private List<DestructionCell> _neighboringCells;
        [SerializeField] private bool _isFoundation = false;

        private Rigidbody _rigidbody;

        public bool IsBreaked { get; private set; }
        public bool IsFoundation => _isFoundation;
        public int ConnectionsCount => _neighboringCells.Count;

        private void Start()
        {
            _neighboringCells = new();
            _rigidbody = GetComponent<Rigidbody>();

            IsBreaked = false;
        }

        private void OnDrawGizmos()
        {
            if (_neighboringCells == null)
                return;

            if (_isFoundation)
                Gizmos.color = Color.blue;
            else if (IsConnectedToFondation(new()) && IsBreaked == false)
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

        public void Clear() =>
            _neighboringCells.Clear();

        public bool Contains(DestructionCell destructionCell) =>
            _neighboringCells.Contains(destructionCell);

        public void AddNeighboring(DestructionCell destructionCell)
        {
            if (Contains(destructionCell) == false)
                _neighboringCells.Add(destructionCell);
        }

        public void TakeDamage(ExplosionInfo explosionInfo) =>
            Destruct(explosionInfo.ExplosionPosition, explosionInfo.ExplosionForce);

        public void ReportDestruction(List<DestructionCell> checkedCells, Vector3 bulletPosition, uint explosionForce)
        {
            if (IsConnectedToFondation(checkedCells) == false)
                Destruct(bulletPosition, explosionForce);
        }

        public bool IsConnectedToFondation(List<DestructionCell> checkedCells)
        {
            if (_isFoundation)
                return true;

            checkedCells.Add(this);

            foreach (DestructionCell destructionCell in _neighboringCells)
            {
                if (destructionCell == null)
                    continue;
                else if (checkedCells.Contains(destructionCell) || destructionCell.IsBreaked)
                    continue;
                else if (destructionCell.IsConnectedToFondation(checkedCells))
                    return true;
            }

            return false;
        }

        public override void Destruct(Vector3 bulletPosition, uint explosionForce)
        {
            if (IsBreaked || IsFoundation)
                return;

            IsBreaked = true;

            foreach (DestructionCell neigboringCell in _neighboringCells)
                neigboringCell.ReportDestruction(new(), bulletPosition, explosionForce);

            base.Destruct(bulletPosition, explosionForce);
        }
    }
}