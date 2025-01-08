using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Helicopter
{
    public class HelicopterBlades : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private RotationAxis _axis;

        private float _rotateDegree;
        private Vector3 _startRotation;

        private void Start()
        {
            _startRotation = new Vector3(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        }

        private void Update()
        {
            _rotateDegree += _speed * Time.deltaTime;
            _rotateDegree = _rotateDegree % 360;

            transform.localRotation = Quaternion.Euler((GetAxis() * _rotateDegree) + _startRotation);
        }

        private Vector3 GetAxis()
        {
            switch (_axis)
            {
                case RotationAxis.X:
                     return Vector3.right;
                case RotationAxis.Y:
                    return Vector3.up;
                case RotationAxis.Z:
                    return Vector3.forward;
                default:
                    return Vector3.zero;
            }
        }

        private enum RotationAxis
        {
            X,
            Y,
            Z,
        }
    }
}
