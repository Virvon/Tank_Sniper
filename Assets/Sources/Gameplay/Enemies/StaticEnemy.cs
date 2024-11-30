using System;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies
{
    public class StaticEnemy : Enemy
    {
        public void Rotate(PlayerTank playerTank, Action callback)
        {
            Vector3 targetDirection = (playerTank.transform.position - transform.position).normalized;
            //Vector3 shootPointForward = ShootPoint.forward;

            targetDirection.y = 0;
            //shootPointForward.y = 0;

            //Quaternion targetRotation = Quaternion.Euler(
            //    0,
            //    transform.rotation.eulerAngles.y + Quaternion.FromToRotation(shootPointForward, targetDirection).eulerAngles.y,
            //    0);

            //StartCoroutine(Rotater(targetRotation, callback));
        }

        private IEnumerator Rotater(Quaternion targetRotation, Action callback)
        {
            Quaternion startRotation = transform.rotation;

            while (transform.rotation != targetRotation)
            {
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

                yield return null;
            }

            callback?.Invoke();
        }
    }
}
