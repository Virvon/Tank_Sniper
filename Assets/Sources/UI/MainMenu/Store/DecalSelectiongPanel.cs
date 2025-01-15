using Assets.Sources.Data;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.UI.MainMenu.Store
{
    public class DecalSelectiongPanel : SelectionPanel<string>
    {
        [SerializeField] private int _tankRotationAngle;
        [SerializeField] private UiSelectedTankPoint _tankPoint;

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

            foreach (DecalData decalData in persistentProgressService.Progress.Decals)
            {
                SelectingPanelElement panel = await uiFactory.CreateUnlockingPanel(content);

                panel.Initialize(decalData.Id.ToString());

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
                Debug.Log("Reward video");
                persistentProgressService.Progress.UnlockDecal(key);
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
