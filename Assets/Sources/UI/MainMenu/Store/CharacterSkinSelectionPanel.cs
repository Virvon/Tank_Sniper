using Assets.Sources.Data;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.UI.MainMenu.Store
{
    public class CharacterSkinSelectionPanel : SelectionPanel<PlayerCharacterType>
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

        protected override async UniTask<Dictionary<PlayerCharacterType, SelectingPanelElement>> FillContent(
            IUiFactory uiFactory,
            IPersistentProgressService persistentProgressService,
            Transform content)
        {
            Dictionary<PlayerCharacterType, SelectingPanelElement> panels = new();

            foreach(CharacterSkinData skinData in persistentProgressService.Progress.CharacterSkins)
            {
                SelectingPanelElement panel = await uiFactory.CreateCharacterSkinPanel(content);

                panel.Initialize(skinData.Type.ToString());

                if (skinData.IsUnlocked)
                    panel.Unlock();

                panel.Clicked += OnPanelClicked;

                panels.Add(skinData.Type, panel);
            }

            return panels;
        }

        protected override void Select(PlayerCharacterType key, IPersistentProgressService persistentProgressService)
        {
            CharacterSkinData skinData = persistentProgressService.Progress.GetCharacterSkin(key);

            if(skinData.IsUnlocked == false)
            {
                Debug.Log("reward video");
                persistentProgressService.Progress.UnlockCharacterSkin(key);
            }
            else
            {
                persistentProgressService.Progress.SelectCharacterSkin(key);
            }

            ActiveSelectionFrame(key);
        }

        protected override void Subscribe(IPersistentProgressService persistentProgressService) =>
            persistentProgressService.Progress.CharacterSkinUnlocked += Unlock;

        protected override void Unsubscribe(IPersistentProgressService persistentProgressService) =>
            persistentProgressService.Progress.CharacterSkinUnlocked -= Unlock;

        protected override PlayerCharacterType GetCurrentSelectedPanel(IPersistentProgressService persistentProgressService) =>
            persistentProgressService.Progress.SelectedPlayerCharacter;
    }
}
