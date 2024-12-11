using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.MainMenu
{
    public class TankPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private CanvasGroup _fon;

        public void Initialize(string text)
        {
            _text.text = text;
        }
        
        public void Unlock()
        {
            _fon.alpha = 1;
            _fon.blocksRaycasts = true;
            _fon.interactable = true;
        }

        public class Factory : PlaceholderFactory<string, Transform, UniTask<TankPanel>>
        {
        }
    }
}
