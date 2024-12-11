using UnityEngine;

namespace Assets.Sources.MainMenu
{
    public class MainMenuCamera : MonoBehaviour
    {
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private Camera _mainCamera;

        public Camera UiCamera => _uiCamera;
        public Camera MainCamera => _mainCamera;
    }
}
