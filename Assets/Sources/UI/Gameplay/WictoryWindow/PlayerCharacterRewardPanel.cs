using Assets.Sources.Data;
using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Infrastructure.Factories.TankFactory;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Tanks;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Gameplay.WictoryWindow
{
    public class PlayerCharacterRewardPanel : MonoBehaviour
    {
        private const string Layer = "CharacterRewardWictoryWindow";

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _collectButton;
        [SerializeField] private Transform _characterPoint;
        [SerializeField] private float _characterScale;
        [SerializeField] private float _characterRotationSpeed;
        [SerializeField] private TMP_Text _text;

        private IPersistentProgressService _persistentProgressService;
        private IStaticDataService _staticDataService;
        private IAssetProvider _assetProvider;
        private ITankFactory _tankFactory;

        private string _characterId;
        private PlayerCharacter _playerCharacter;

        [Inject]
        private void Construct(
            IPersistentProgressService persistentProgressService,
            IStaticDataService staticDataService,
            IAssetProvider assetProvider,
            ITankFactory tankFactory)
        {
            _persistentProgressService = persistentProgressService;
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
            _tankFactory = tankFactory;


            _collectButton.onClick.AddListener(OnCollectButtonClicked);
        }

        private void OnDestroy() =>
            _collectButton.onClick.RemoveListener(OnCollectButtonClicked);

        private void OnCollectButtonClicked()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            _persistentProgressService.Progress.GetPlayerCharacter(_characterId).IsBuyed = true;

            Hide();
#else
            Agava.YandexGames.InterstitialAd.Show(onCloseCallback: (value) =>
            {
                _persistentProgressService.Progress.GetPlayerCharacter(_characterId).IsBuyed = true;

                Hide();
            });
#endif
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
            _persistentProgressService.Progress.GetPlayerCharacter(_characterId).IsUnlocked = true;

            _playerCharacter = await _tankFactory.CreatePlayerCharacter(_characterId, _characterPoint.position, Quaternion.identity, _characterPoint);

            foreach (Transform transform in _playerCharacter.GetComponentsInChildren<Transform>())
                transform.gameObject.layer = LayerMask.NameToLayer(Layer);

            _text.text = "generated " + _characterId;

            StartCoroutine(Rotater());
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        private IEnumerator Rotater()
        {
            bool isRotated = true;

            float rotation = _characterPoint.rotation.eulerAngles.y;

            while (isRotated)
            {
                rotation += _characterRotationSpeed * Time.deltaTime;

                _characterPoint.rotation = Quaternion.Euler(0, rotation, 0);

                yield return null;
            }
        }
    }
}
