﻿using Assets.Sources.Gameplay.Bullets;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.Weapons
{
    public class TankRocket : MonoBehaviour
    {
        private readonly Collider[] _overlapColliders = new Collider[32];

        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private GameObject _projectile;
        [SerializeField] private Collider _collider;
        [SerializeField] private ParticleSystem _explosionParticlePrefab;

        private uint _explosionRadius;
        private uint _flightSpeed;
        private float _explosionLifeTime;
        private uint _explosionForce;

        public void Initialize(uint explosionRadius, uint flightSpeed, float explosionLifeTime, uint explosionForce)
        {
            _explosionRadius = explosionRadius;
            _flightSpeed = flightSpeed;
            _explosionLifeTime = explosionLifeTime;
            _explosionForce = explosionForce;

            _rigidBody.velocity = transform.forward * _flightSpeed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            List<IDamageable> hitedTargets = new();

            _rigidBody.velocity = Vector3.zero;
            _rigidBody.isKinematic = true;
            _collider.enabled = false;

            ParticleSystem explosionParticle = Instantiate(_explosionParticlePrefab, transform.position, Quaternion.identity, transform);
            explosionParticle.Play();

            Destroy(_projectile);

            if(collision.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(transform.position, _explosionForce);
                hitedTargets.Add(damageable);
            }

            int overlapCount = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, _overlapColliders);

            for (int i = 0; i < overlapCount; i++)
            {
                if (_overlapColliders[i].TryGetComponent(out damageable) && hitedTargets.Contains(damageable) == false)
                {
                    damageable.TakeDamage(transform.position, _explosionForce);
                    hitedTargets.Add(damageable);
                }
            }

            Destroy(gameObject, _explosionLifeTime);
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<TankRocket>>
        {
        }
    }
}
