﻿using Assets.Sources.Gameplay.Destructions;
using Assets.Sources.Gameplay.Enemies.Animation;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies
{
    public class DestructedEnemy : Enemy
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private DestructionPart _weapon;
        [SerializeField] private Collider _weaponCollider;
        [SerializeField] private Ragdoll _ragdoll;

        public void Destruct(Vector3 explosionPosition, uint explosionForce)
        {
            OnDestructed();

            _animator.enabled = false;

            _ragdoll.transform.parent = null;
            _ragdoll.Destruct(explosionPosition, explosionForce);

            _weaponCollider.enabled = true;
            _weapon.transform.parent = null;
            _weapon.Destruct(explosionPosition, explosionForce);

            Destroy(gameObject);
        }
    }
}