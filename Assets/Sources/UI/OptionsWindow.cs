using Assets.Sources.Services.PersistentProgress;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI
{
    public class OptionsWindow : OpenableWindow
    {
        private const string Volume = "Volume";

        [SerializeField] private Button _hideButton;
        [SerializeField] private Button _activeSoundsButton;
        [SerializeField] private CanvasGroup _activeSoundsButtonCanvasGroup;
        [SerializeField] private Button _deactiveSoundsButton;
        [SerializeField] private CanvasGroup _deactiveSoundsButtonCanvasGroup;
        [SerializeField] private AudioMixer _audioMixer;

        private IPersistentProgressService _persistentProgressService;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService)
        {
            _persistentProgressService = persistentProgressService;

            if (_persistentProgressService.Progress.IsSoundOn)
                OnActiveSoundsButtonClicked();
            else
                OnDeactiveSoundsButtonClicked();

            _hideButton.onClick.AddListener(Hide);
            _activeSoundsButton.onClick.AddListener(OnActiveSoundsButtonClicked);
            _deactiveSoundsButton.onClick.AddListener(OnDeactiveSoundsButtonClicked);
        }

        private void OnDestroy()
        {
            _hideButton.onClick.RemoveListener(Hide);
            _activeSoundsButton.onClick.RemoveListener(OnActiveSoundsButtonClicked);
            _deactiveSoundsButton.onClick.RemoveListener(OnDeactiveSoundsButtonClicked);
        }

        public override void Show()
        {
            base.Show();
            Time.timeScale = 0;
        }

        public override void Hide()
        {
            base.Hide();
            Time.timeScale = 1;
        }

        private void SetCanvasGroupActive(CanvasGroup canvasGroup, bool isActive)
        {
            canvasGroup.alpha = isActive ? 1 : 0;
            canvasGroup.blocksRaycasts = isActive;
            canvasGroup.interactable = isActive;
        }

        private void OnDeactiveSoundsButtonClicked()
        {
            _audioMixer.SetFloat(Volume, -80);
            _persistentProgressService.Progress.IsSoundOn = false;

            SetCanvasGroupActive(_activeSoundsButtonCanvasGroup, true);
            SetCanvasGroupActive(_deactiveSoundsButtonCanvasGroup, false);
        }

        private void OnActiveSoundsButtonClicked()
        {
            _audioMixer.SetFloat(Volume, 0);
            _persistentProgressService.Progress.IsSoundOn = true;

            SetCanvasGroupActive(_activeSoundsButtonCanvasGroup, false);
            SetCanvasGroupActive(_deactiveSoundsButtonCanvasGroup, true);
        }

    }
}
