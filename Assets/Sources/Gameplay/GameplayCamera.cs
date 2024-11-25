using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class GameplayCamera : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<string, UniTask<GameplayCamera>>
        {
        }
    }
}
