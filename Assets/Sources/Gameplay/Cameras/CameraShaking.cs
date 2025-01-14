using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Cameras
{
    public class CameraShaking : MonoBehaviour
    {
        private const float DisabledFrequencyGain = 0;

        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private float _shakeDuration;

        private CinemachineBasicMultiChannelPerlin _noise;
        private float _frequencyGain;
        private Coroutine _timer;

        private void Start()
        {
            _noise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _frequencyGain = _noise.m_FrequencyGain;
            _noise.m_FrequencyGain = DisabledFrequencyGain;
        }

        public void Shake()
        {
            _noise.m_FrequencyGain = _frequencyGain;

            if (_timer != null)
                StopCoroutine(_timer);

            _timer = StartCoroutine(Timer(() => _noise.m_FrequencyGain = DisabledFrequencyGain));
        }

        private IEnumerator Timer(Action callback)
        {
            yield return new WaitForSeconds(_shakeDuration);

            callback?.Invoke();
        }
    }
}