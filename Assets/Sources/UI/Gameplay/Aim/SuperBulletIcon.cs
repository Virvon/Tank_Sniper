using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Gameplay.Aim
{
    public class SuperBulletIcon : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _icon;

        private void Start() =>
            SetAcive(false);

        public void SetAcive(bool isActive) =>
            _icon.alpha = isActive ? 1 : 0;

        public class Factory : PlaceholderFactory<string, Transform, UniTask<SuperBulletIcon>>
        {
        }
    }
}
