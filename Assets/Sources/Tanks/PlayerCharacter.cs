using Assets.Sources.Gameplay.Enemies.Animation;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Tanks
{
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField] private Transform _glassesPoint;
        [SerializeField] private GameObject _glasses;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _controllerPoint;

        public Transform GlassesPoint => _glassesPoint;
        public Transform ControllerPoint => _controllerPoint;

        public void TryDestroyGlasses()
        {
            if (_glasses != null)
                Destroy(_glasses);
        }

        public void StartDroneControlling() =>
            _animator.SetBool(AnimationPath.IsControllingDrone, true);

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, Transform, UniTask<PlayerCharacter>>
        {
        }
    }
}
