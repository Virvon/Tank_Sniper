using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.MainMenu.Store
{
    public abstract class SelectionPanel<TKey> : StorePanelTab
    {
        [SerializeField] private Transform _content;

        private IPersistentProgressService _persistentProgressService;
        private IUiFactory _uiFactory;

        private Dictionary<TKey, SelectingPanelElement> _panels;
        private SelectingPanelElement _currentSelectingElement;

        public IPersistentProgressService PersistentProgressService => _persistentProgressService;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, IUiFactory uiFactory)
        {
            _persistentProgressService = persistentProgressService;
            _uiFactory = uiFactory;

            Subscribe(_persistentProgressService);

            _persistentProgressService.Progress.SelectedTankChanged += OnSelectedTankChanged;
        }

        private async void Start()
        {
            _panels = await FillContent(_uiFactory, _persistentProgressService, _content);

            ActiveSelectionFrame(GetCurrentSelectedPanel(_persistentProgressService));
        }

        private void OnDestroy()
        {
            Unsubscribe(_persistentProgressService);

            if(_panels != null && _panels.Count > 0)
            {
                foreach (SelectingPanelElement panel in _panels.Values)
                    panel.Clicked -= OnPanelClicked;
            }

            _persistentProgressService.Progress.SelectedTankChanged -= OnSelectedTankChanged;
        }

        protected void Unlock(TKey key) =>
            _panels[key].Unlock();

        protected abstract void Unsubscribe(IPersistentProgressService persistentProgressService);

        protected abstract void Subscribe(IPersistentProgressService persistentProgressService);

        protected abstract UniTask<Dictionary<TKey, SelectingPanelElement>> FillContent(
            IUiFactory uiFactory,
            IPersistentProgressService persistentProgressService,
            Transform content);

        protected abstract void Select(TKey key, IPersistentProgressService persistentProgressService);

        protected abstract TKey GetCurrentSelectedPanel(IPersistentProgressService persistentProgressService);

        protected void ActiveSelectionFrame(TKey key)
        {
            _currentSelectingElement?.SetSelectionFrameActive(false);
            
            if(_panels.TryGetValue(key, out _currentSelectingElement))
                _currentSelectingElement.SetSelectionFrameActive(true);
        }

        protected void OnPanelClicked(SelectingPanelElement panel)
        {
            TKey key = _panels.Keys.First(key => _panels[key] == panel);

            Select(key, _persistentProgressService);
        }

        private void OnSelectedTankChanged(uint level) =>
            ActiveSelectionFrame(GetCurrentSelectedPanel(_persistentProgressService));
    }
}
