using Assets.Sources.Data;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Gameplay.WictoryWindow
{
    public class PlayerCharacterRewardPanel : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _collectButton;

        private IPersistentProgressService _persistentProgressService;
        private IStaticDataService _staticDataService;
        private IAssetProvider _assetProvider;

        string _characterId;

        [Inject]
        private async void Construct(IPersistentProgressService persistentProgressService, IStaticDataService staticDataService, IAssetProvider assetProvider)
        {
            _persistentProgressService = persistentProgressService;
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;

            await GenerateCharacter();

            _collectButton.onClick.AddListener(OnCollectButtonClicked);
        }

        private void OnDestroy() =>
            _collectButton.onClick.RemoveListener(OnCollectButtonClicked);

        private void OnCollectButtonClicked()
        {
            _persistentProgressService.Progress.GetPlayerCharacter(_characterId).IsUnlocked = true;

            Hide();
        }

        public async UniTask GenerateCharacter()
        {
            PlayerCharacterData[] datas = _persistentProgressService.Progress.PlayerCharacters;

            PlayerCharacterData[] lockedDatas = datas.Where(data => data.IsUnlocked == false).ToArray();

            if (lockedDatas.Length == 0)
            {
                Hide();

                return;
            }

            PlayerCharacterData characterData = lockedDatas[Random.Range(0, lockedDatas.Length)];
            _characterId = characterData.Id;

            Sprite icon= await _assetProvider.Load<Sprite>(_staticDataService.GetPlayerCharacter(_characterId).Icon);
            _icon.sprite = icon;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
