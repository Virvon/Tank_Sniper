using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.MainMenu
{
    public class Tank : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<string, Vector3, Quaternion, Transform, UniTask<Tank>>
        {
        }
    }
}
