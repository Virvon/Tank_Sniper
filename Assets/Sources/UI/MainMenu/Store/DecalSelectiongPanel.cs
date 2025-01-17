using Assets.Sources.Data;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.MainMenu.Store
{
    public class DecalSelectiongPanel : SelectionPanel<string>
    {
        private const string Texture = "_Texture";

        [SerializeField] private int _tankRotationAngle;
        [SerializeField] private UiSelectedTankPoint _tankPoint;

        private IStaticDataService _staticDataService;
        private IAssetProvider _assetProvider;

        [Inject]
        private void Construct(IStaticDataService staticDataService, IAssetProvider assetProvider)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
        }

        public override void Open()
        {
            base.Open();
            _tankPoint.SetTargetRotation(_tankRotationAngle);
        }

        public override void Hide()
        {
            base.Hide();
            _tankPoint.ResetTargetRotation();
        }

        protected override async UniTask<Dictionary<string, SelectingPanelElement>> FillContent(
            IUiFactory uiFactory,
            IPersistentProgressService persistentProgressService,
            Transform content)
        {
            Dictionary<string, SelectingPanelElement> panels = new();
            DecalData[] decalDatas = persistentProgressService.Progress.Decals;

            decalDatas = decalDatas.OrderBy(data => _staticDataService.GetDecal(data.Id).SerialNumber).ToArray();

            foreach (DecalData decalData in decalDatas)
            {
                SelectingPanelElement panel = await uiFactory.CreateDecalPanel(content);

                Sprite sprite = await _assetProvider.Load<Sprite>(_staticDataService.GetDecal(decalData.Id).SpriteAssetReference);

                panel.Initialize(sprite);

                if (decalData.IsUnlocked)
                    panel.Unlock();

                panel.Clicked += OnPanelClicked;

                panels.Add(decalData.Id, panel);
            }

            return panels;
        }

        protected override void Select(string key, IPersistentProgressService persistentProgressService)
        {
            DecalData decalData = persistentProgressService.Progress.GetDecal(key);

            if (decalData.IsUnlocked == false)
            {
#if !UNITY_WEBGL || UNITY_EDITOR
                persistentProgressService.Progress.UnlockDecal(key);
#else
            Agava.YandexGames.InterstitialAd.Show(onCloseCallback: (value) =>
            {
                persistentProgressService.Progress.UnlockDecal(key);
            });
#endif                
            }
            else
            {
                persistentProgressService.Progress.SelectDecal(key);
            }

            ActiveSelectionFrame(key);
        }

        protected override void Subscribe(IPersistentProgressService persistentProgressService) =>
            persistentProgressService.Progress.DecalUnlocked += Unlock;

        protected override void Unsubscribe(IPersistentProgressService persistentProgressService) =>
            persistentProgressService.Progress.DecalUnlocked -= Unlock;

        protected override string GetCurrentSelectedPanel(IPersistentProgressService persistentProgressService) =>
            persistentProgressService.Progress.GetSelectedTank().DecalId;
    }
}
