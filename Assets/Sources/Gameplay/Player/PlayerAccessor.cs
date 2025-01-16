using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Player
{
    public class PlayerAccessor : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<string, Vector3, Quaternion, Transform, UniTask<PlayerAccessor>>
        {
        }
    }
}
