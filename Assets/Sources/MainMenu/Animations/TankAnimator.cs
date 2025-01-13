using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.MainMenu.Animations
{
    public abstract class TankAnimator : MonoBehaviour
    {
        private Coroutine _animator;

        public AnimationsConfig AnimationsConfig { get; private set; }

        [Inject]
        private void Construct(IStaticDataService staticDataService) =>
            AnimationsConfig = staticDataService.AnimationsConfig;

        public void Play()
        {
            if (enabled == false)
                return;

            if (_animator != null)
                StopCoroutine(_animator);

            _animator = StartCoroutine(Animator());
        }

        protected abstract IEnumerator Animator();
    }
}
