using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private float _z = 1;

        private void Update()
        {
            Debug.Log(_camera.WorldToScreenPoint(new Vector3(_gameObject.transform.position.x, _gameObject.transform.position.y, _z)));
        }
    }
}
