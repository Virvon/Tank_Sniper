using Assets.Sources.MainMenu.Animations;
using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.MainMenu.Desk
{
    public class DeskTankWrapper : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tankLevelValue;
        [SerializeField] private TankScalingAnimator _tankScalingAnimator;
        [SerializeField] private Transform _tankPoint;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private AudioSource _audioSource;

        public uint TankLevel { get; private set; }
        public Transform TankPoint => _tankPoint;

        public void Initialize(uint tankLevel)
        {
            TankLevel = tankLevel;
            _tankLevelValue.text = tankLevel.ToString();
        }

        public void Animate(bool needParticles)
        {
            _tankScalingAnimator.Play();
            _audioSource.Play();
            
            if(needParticles)
                _particleSystem.Play();
        }

        public void Destroy() =>
            Destroy(gameObject);

        public class Factory : PlaceholderFactory<string, Vector3, Transform, UniTask<DeskTankWrapper>>
        {
        }       
    }
}
