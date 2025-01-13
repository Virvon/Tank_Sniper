using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using Cysharp.Threading.Tasks;
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

        private Dictionary<TKey, SelectingPanelElement> _panels;

        public IPersistentProgressService PersistentProgressService => _persistentProgressService;

        [Inject]
        private async void Construct(IPersistentProgressService persistentProgressService, IUiFactory uiFactory)
        {
            _persistentProgressService = persistentProgressService;

            Subscribe(_persistentProgressService);
            _panels = await FillContent(uiFactory, _persistentProgressService, _content);
        }

        private void OnDestroy()
        {
            Unsubscribe(_persistentProgressService);

            foreach (SelectingPanelElement panel in _panels.Values)
                panel.Clicked -= OnPanelClicked;
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

        protected void OnPanelClicked(SelectingPanelElement panel)
        {
            TKey key = _panels.Keys.First(key => _panels[key] == panel);

            Select(key, _persistentProgressService);
        }
    }
}
