using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Cameras
{
    public class RotationCamera : HandleableRotationObject
    {
        private float _startRotation;
        private AimingConfig _aimingConfig;

        [Inject]
        private void Construct(IStaticDataService staticDataService)
        {
            _aimingConfig = staticDataService.AimingConfig;

            _startRotation = transform.rotation.eulerAngles.y;
            ResetRotation();
        }

        public void ResetRotation()
        {
            Rotation = _aimingConfig.StartRotation + new Vector2(0, _startRotation);
            Rotate();
        }

        protected override Vector2 ClampRotation(Vector2 rotation)
        {
            return new Vector2(
                Mathf.Clamp(rotation.x, _aimingConfig.MinRotation.x, _aimingConfig.MaxRotation.x),
                Mathf.Clamp(rotation.y, _aimingConfig.MinRotation.y + _startRotation, _aimingConfig.MaxRotation.y + _startRotation));
        }

        public class Factory : PlaceholderFactory<string, Vector3, Quaternion, UniTask<RotationCamera>>
        {
        }
    }
}