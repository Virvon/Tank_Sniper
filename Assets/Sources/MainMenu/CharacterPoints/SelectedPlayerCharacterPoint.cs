using Assets.Sources.Infrastructure.Factories.TankFactory;
using Assets.Sources.MainMenu.Animations;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Tanks;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.MainMenu.CharacterPoints
{
    public class SelectedPlayerCharacterPoint : MonoBehaviour
    {
        private const string Layer = "UI";

        [SerializeField] private Transform _characterPoint;
        [SerializeField] private TankScalingAnimator _scalingAnimator;
        [SerializeField] private float _scale;

        private IPersistentProgressService _persistentPorgressService;
        private ITankFactory _tankFactory;

        private PlayerCharacter _playerCharacter;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, ITankFactory tankFactory)
        {
            _persistentPorgressService = persistentProgressService;
            _tankFactory = tankFactory;

            _persistentPorgressService.Progress.CharacterSkinChanged += OnCharacterSkinChanged;
        }

        private async void Start() =>
            await CreatePlayerCharacter(_persistentPorgressService.Progress.SelectedPlayerCharacter, false);

        private void OnDestroy() =>
            _persistentPorgressService.Progress.CharacterSkinChanged -= OnCharacterSkinChanged;

        private async void OnCharacterSkinChanged(PlayerCharacterType type) =>
            await CreatePlayerCharacter(type, true);

        private async UniTask CreatePlayerCharacter(PlayerCharacterType type, bool needToAnimate)
        {
            if (_playerCharacter != null)
                Destroy(_playerCharacter.gameObject);

            _playerCharacter = await _tankFactory.CreatePlayerCharacter(type, _characterPoint.position, _characterPoint.rotation, _characterPoint);

            foreach (Transform transform in _playerCharacter.GetComponentsInChildren<Transform>())
                transform.gameObject.layer = LayerMask.NameToLayer(Layer);

            _playerCharacter.transform.localScale = Vector3.one * _scale;

            if (needToAnimate)
                _scalingAnimator.Play();
        }
    }
}