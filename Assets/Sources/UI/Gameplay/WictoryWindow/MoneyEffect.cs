using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Gameplay.WictoryWindow
{
    public class MoneyEffect : MonoBehaviour
    {
        [SerializeField] private MoneyParticle _moneyParticlePrefab;
        [SerializeField] private Transform _starPoint;
        [SerializeField] private Transform _targetPoint;
        [SerializeField] private int _particlesCount;

        private AnimationsConfig _animationConfig;

        [Inject]
        private void Construct(IStaticDataService staticDataService) =>
            _animationConfig = staticDataService.AnimationsConfig;

        public void Play()
        {
            for(int i =0; i < _particlesCount; i++)
            {
                MoneyParticle moneyParticle = Instantiate(_moneyParticlePrefab, _starPoint.position, Quaternion.identity, transform);
                moneyParticle.Initialize(_starPoint.position, _targetPoint.position, _animationConfig);
            }
        }
    }
}
