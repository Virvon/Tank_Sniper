using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.LoadingCurtain
{
    public class LoadingCurtain : MonoBehaviour, ILoadingCurtain
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _showDuration;

        public void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            StartCoroutine(Timer(callback: () =>
            {
                _canvasGroup.alpha = 0;
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
            }));
        }

        private IEnumerator Timer(Action callback)
        {
            yield return new WaitForSeconds(_showDuration);

            callback?.Invoke();
        }

        public class Factory : PlaceholderFactory<string, UniTask<LoadingCurtain>>
        {
        }
    }
}
