using Assets.Sources.Services.InputService;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class Aiming : MonoBehaviour
    {
        [SerializeField] private float _sensivity = 1;

        private IInputService _inputService;

        private Vector2 _rotation;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;

            _rotation = new Vector2(transform.rotation.w, transform.rotation.y);

            _inputService.SightShifted += OnSighhtShifted;
        }

        private void OnDestroy()
        {
            _inputService.SightShifted -= OnSighhtShifted;
        }

        private void OnSighhtShifted(Vector2 delta)
        {
            _rotation += new Vector2(-delta.y, delta.x) * _sensivity;
            transform.rotation = Quaternion.Euler(_rotation.x, _rotation.y, 0);
        }
    }
}
