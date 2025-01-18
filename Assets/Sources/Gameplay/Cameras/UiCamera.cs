using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Cameras
{
    public class UiCamera : MonoBehaviour
    {
        [SerializeField] private Camera _uiCamera;

        public Camera Camera => _uiCamera;

        public class Factory : PlaceholderFactory<string, UniTask<UiCamera>>
        {
        }
    }
}