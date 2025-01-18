using Assets.Sources.Gameplay.Destructions;
using Assets.Sources.Gameplay.Destructions.Building;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies
{
    public class BuildingDestructedEnemyCharacter : DestructedEnemy
    {
        private const float Radius = 4;

        private readonly Collider[] _overlapColliders = new Collider[32];

        private IDestructablePart _destructablePart;

        private void Start()
        {
            int overlapCount = Physics.OverlapSphereNonAlloc(transform.position, Radius, _overlapColliders);

            for (int i = 0; i < overlapCount; i++)
            {
                if (_overlapColliders[i].TryGetComponent(out IDestructablePart destructablePart))
                {
                    if (_destructablePart == null)
                        _destructablePart = destructablePart;
                    else if (Vector3.Distance(destructablePart.Transform.GetComponent<Collider>().ClosestPoint(transform.position), transform.position) < Vector3.Distance(_destructablePart.Transform.GetComponent<Collider>().ClosestPoint(transform.position), transform.position))
                        _destructablePart = destructablePart;
                }
            }

            if (_destructablePart == null)
                Debug.LogError($"{typeof(IDestructablePart)} not founded");

            _destructablePart.Destructed += OnPartDestructed;
        }

        private void OnDestroy() =>
            _destructablePart.Destructed -= OnPartDestructed;

        private void OnPartDestructed(Vector3 explosionPosition, uint explosionForce)
        {
            Debug.Log("destructed");

            Destruct(explosionPosition, explosionForce);
        }
    }
}
