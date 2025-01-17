using Assets.Sources.MainMenu.Animations;
using Assets.Sources.MainMenu.Weapons;
using Assets.Sources.Services.InputService;
using Assets.Sources.UI.MainMenu;
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

        private TankShootingHandler _tankShootingHandler;
        private MainMenuCamera _camera;

        [Inject]
        private void Construct(TankShootingHandler tankShootingHandler, MainMenuCamera camera)
        {
            _tankShootingHandler = tankShootingHandler;
            _camera = camera;

            _tankShootingHandler.HandlePressed += OnHandlePressed;
        }

        private void OnDestroy() =>
            _tankShootingHandler.HandlePressed -= OnHandlePressed;

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
