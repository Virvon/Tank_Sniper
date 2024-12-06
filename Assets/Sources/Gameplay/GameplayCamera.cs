using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class GameplayCamera : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        public Camera Camera => _camera;

        public class Factory : PlaceholderFactory<string, UniTask<GameplayCamera>>
        {
        }
    }
}
