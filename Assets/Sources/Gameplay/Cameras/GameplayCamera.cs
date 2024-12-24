using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Cameras
{
    public class GameplayCamera : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private CinemachineBrain _cinemachineBrain;

        public Camera Camera => _camera;

        public void SetBlednDuration(float duration) =>
            _cinemachineBrain.m_DefaultBlend = new CinemachineBlendDefinition(_cinemachineBrain.m_DefaultBlend.m_Style, duration);

        public class Factory : PlaceholderFactory<string, UniTask<GameplayCamera>>
        {
        }
    }
}
