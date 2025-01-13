using Cysharp.Threading.Tasks;
using MPUIKIT;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI
{
    public class ProgressBarElement : MonoBehaviour
    {
        [SerializeField] private MPImage _image;

        public void Initialize(Color color) =>
            _image.color = color;

        public class Factory : PlaceholderFactory<string, Transform, UniTask<ProgressBarElement>>
        {
        }
    }
}
