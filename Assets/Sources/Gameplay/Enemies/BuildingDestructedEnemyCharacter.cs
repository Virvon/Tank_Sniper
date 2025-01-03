using Assets.Sources.Gameplay.Destructions.Building;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies
{
    public class BuildingDestructedEnemyCharacter : DestructedEnemy
    {
        private const float Radius = 3;

        private readonly Collider[] _overlapColliders = new Collider[32];

        private DestructionCell _destructionCell;

        private void Start()
        {
            int overlapCount = Physics.OverlapSphereNonAlloc(transform.position, Radius, _overlapColliders);

            for (int i = 0; i < overlapCount; i++)
            {
                if (_overlapColliders[i].TryGetComponent(out DestructionCell destructionCell))
                {
                    if (_destructionCell == null)
                        _destructionCell = destructionCell;
                    else if (Vector3.Distance(destructionCell.transform.position, transform.position) < Vector3.Distance(_destructionCell.transform.position, transform.position))
                        _destructionCell = destructionCell;
                }
            }

            if (_destructionCell == null)
                Debug.LogError($"{typeof(DestructionCell)} not founded");

            _destructionCell.Destructed += Destruct;
        }

        private void OnDestroy() =>
            _destructionCell.Destructed -= Destruct;
    }
}
