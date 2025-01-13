using UnityEngine;

namespace Assets.Sources.MainMenu.Desk
{
    public class Marker : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Color _negativeColor;
        [SerializeField] private Color _positiveColor;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void Mark(bool value)
        {
            _meshRenderer.materials[0].color = value ? _positiveColor : _negativeColor;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
