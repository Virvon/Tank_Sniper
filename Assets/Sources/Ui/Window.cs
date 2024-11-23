using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Ui
{
    public class Window : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<string, UniTask<Window>>
        {
        }
    }
}
