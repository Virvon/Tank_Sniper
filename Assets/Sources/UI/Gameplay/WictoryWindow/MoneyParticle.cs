using Assets.Sources.Services.StaticDataService.Configs;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Sources.UI.Gameplay.WictoryWindow
{
    public class MoneyParticle : MonoBehaviour
    {
        public void Initialize(Vector3 startPosition, Vector3 targetPosition, AnimationsConfig animationsConfig)
        {
            Vector2 explosionPosition = Random.insideUnitCircle * animationsConfig.MoneyEffectRadius + new Vector2(startPosition.x, startPosition.y);

            StartCoroutine(Animator(animationsConfig.MoneyEffectDelay, new Vector2(startPosition.x, startPosition.y), explosionPosition, callback: () =>
            {
                StartCoroutine(Animator(animationsConfig.WalletValueChangingDuration - animationsConfig.MoneyEffectDelay, explosionPosition, targetPosition, callback: () =>
                {
                    Destroy(gameObject);
                }));
            }));
        }

        private IEnumerator Animator(float duration, Vector2 startPosition, Vector2 targetPosition, Action callback = null)
        {
            float progress;
            float passedTime = 0;
            bool isFinished = false;

            while (isFinished == false)
            {
                progress = Mathf.Min(passedTime / duration, 1);
                passedTime += Time.deltaTime;

                Vector2 position = Vector2.Lerp(startPosition, targetPosition, progress);
                transform.position = new Vector3(position.x, position.y, transform.position.z);

                if (progress == 1)
                    isFinished = true;

                yield return null;
            }

            callback?.Invoke();
        }
    }
}
