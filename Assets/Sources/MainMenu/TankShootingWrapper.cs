using Assets.Sources.MainMenu.Animations;
using Assets.Sources.MainMenu.Weapons;
using Assets.Sources.Services.InputService;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.MainMenu
{
    public class TankShootingWrapper : MonoBehaviour
    {
        private const uint MaxRaycastDistance = 200;

        [SerializeField] private Weapon _weapon;
        [SerializeField] private ShootingAnimator _shootingAnimator;

        private IInputService _inputService;
        private MainMenuCamera _camera;

        [Inject]
        private void Construct(IInputService inputService, MainMenuCamera camera)
        {
            _inputService = inputService;
            _camera = camera;

            _inputService.HandlePressed += OnHandlePressed;
        }

        private void OnDestroy() =>
            _inputService.HandlePressed -= OnHandlePressed;

        public void SetBulletPoints(Transform[] bulletPoints) =>
            _weapon.SetBulletPoints(bulletPoints);

        private void OnHandlePressed(Vector2 handlePosition)
        {
            if (Physics.Raycast(_camera.GetRay(handlePosition), out RaycastHit hitInfo, MaxRaycastDistance)
                && hitInfo.transform.gameObject == gameObject
                && _weapon.IsShooted == false)
            {
                _weapon.Shoot(shooted: ()=> _shootingAnimator.Play());
            }
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, Transform, UniTask<TankShootingWrapper>>
        {
        }
    }
}
