using System.Collections;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class OpenableWindow : Window
    {
        [SerializeField] private CanvasGroup _mainCanvasGroup;
        [SerializeField] private float _closeDuration;
        [SerializeField] protected float _openDuration;
        [SerializeField] private bool _isOpenedOnAwake;

        private Coroutine _swithcer;

        private void Start()
        {
            _mainCanvasGroup.blocksRaycasts = _isOpenedOnAwake;
            _mainCanvasGroup.interactable = _isOpenedOnAwake;
            _mainCanvasGroup.alpha = _isOpenedOnAwake ? 1 : 0;
        }

        public virtual void Show()
        {
            _mainCanvasGroup.blocksRaycasts = true;
            _mainCanvasGroup.interactable = true;

            Switch(1, _openDuration);
        }

        public virtual void Hide()
        {
            _mainCanvasGroup.blocksRaycasts = false;
            _mainCanvasGroup.interactable = false;

            Switch(0, _closeDuration);
        }

        public void Switch(float targetAlpha, float duration)
        {
            if (_swithcer != null)
                StopCoroutine(_swithcer);

            _swithcer = StartCoroutine(Swither(targetAlpha, duration));
        }

        private IEnumerator Swither(float targetAlpha, float duration)
        {
            float startAlpha = _mainCanvasGroup.alpha;
            float passedTime = 0;
            float progress;

            while (_mainCanvasGroup.alpha != targetAlpha)
            {
                progress = passedTime / duration;
                passedTime += Time.deltaTime;

                _mainCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, progress);

                yield return null;
            }
        }
    }
}
