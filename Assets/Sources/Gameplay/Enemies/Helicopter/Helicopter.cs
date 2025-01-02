using Assets.Sources.Gameplay.Bullets;
using Assets.Sources.Gameplay.Destructions;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Helicopter
{
    public class Helicopter : Enemy
    {
        [SerializeField] private HelicopterPart _helicopterPart;
        [SerializeField] private EnemyEngineryExplosion _explosion;
        [SerializeField] private DestructionPart[] _destructionParts;
        [SerializeField] private DestructionedMaterialsRenderer _destructionMaterialsRenderer;
        [SerializeField] private Vector3 _firePartilcePosition;
        [SerializeField] private GameObject _firePargiclePrefab;

        private bool _isDestructed;
        private bool _isRotated;

        private GameObject _fireParticle;

        private void Start()
        {
            _isDestructed = false;

            _helicopterPart.Damaged += OnDamaged;
        }

        private void OnDestroy() =>
            _helicopterPart.Damaged -= OnDamaged;

        private void OnDamaged(ExplosionInfo explosionInfo)
        {
            OnDestructed();
            StartCoroutine(Rotater());
            _isDestructed = true;
            _destructionMaterialsRenderer.Render();
            _fireParticle = Instantiate(_firePargiclePrefab, transform.position + _firePartilcePosition, Quaternion.identity, transform);
        }      

        private void OnCollisionEnter(Collision collision)
        {
            if (_isDestructed == false || collision.transform.TryGetComponent(out CollidingBullet _))
                return;

            _isRotated = false;

            foreach (DestructionPart destructionPart in _destructionParts)
            {
                destructionPart.transform.parent = null;
                destructionPart.Destruct(transform.position, _explosion.ExplosionForce);
            }

            _explosion.Explode();
            Destroy(_fireParticle);
        }

        private IEnumerator Rotater()
        {
            _isRotated = true;

            float startRotation = transform.localRotation.eulerAngles.y;
            float rotationDegree = startRotation;

            while (_isRotated)
            {
                rotationDegree += 90 * Time.deltaTime;
                rotationDegree = rotationDegree % 360;

                transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, rotationDegree, transform.localRotation.eulerAngles.z);

                yield return null;
            }
        }
    }
}
