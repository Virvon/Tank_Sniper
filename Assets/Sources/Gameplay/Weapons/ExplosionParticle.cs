using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Assets.Sources.Gameplay.Weapons
{
    public class ExplosionParticle : MonoBehaviour
    {
        [SerializeField] private float _lifeTime;
        //[SerializeField] private float _startRadius;
        [SerializeField] private float _targetRadius;
        [SerializeField] private ParticleSystem[] _sizableParticles;
        [SerializeField] private MinMaxCurve _curve;

        private void OnValidate()
        {
            foreach(ParticleSystem particle in _sizableParticles)
            {
                //SizeOverLifetimeModule sizeOverLifetimeModule = particle.sizeOverLifetime;
                //MinMaxCurve curve = sizeOverLifetimeModule.size;
                //curve.curveMultiplier = _targetRadius / _startRadius;

                MainModule mainModul = particle.main;
                mainModul.startSize = _targetRadius * 2;
                mainModul.startLifetime = _lifeTime;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _targetRadius);
        }
    }
}
