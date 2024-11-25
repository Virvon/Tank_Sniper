using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private float _speed;

        private void Start()
        {
            _rigidBody.velocity = transform.forward * _speed;
            
        }

        private void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }

        public class Factory : PlaceholderFactory<string, Vector3, Quaternion, UniTask<Bullet>>
        {
        }
    }
}
