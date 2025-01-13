using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Gameplay.BulletsPanel
{
    public class BulletIcon : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<string, Transform, UniTask<BulletIcon>>
        {
        }
    }
}
