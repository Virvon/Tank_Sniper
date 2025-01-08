using Assets.Sources.Services.CoroutineRunner;
using Assets.Sources.Services.InputService;
using Assets.Sources.Services.StaticDataService;
using System;

namespace Assets.Sources.Gameplay.Handlers
{
    public class DefeatHandler : GameplayHandler
    {        
        private readonly IInputService _inputService;

        public DefeatHandler(ICoroutineRunner coroutineRunner, IStaticDataService staticDataService, IInputService inputService)
            : base(coroutineRunner, staticDataService) =>
            _inputService = inputService;

        public event Action Defeated;
        public event Action WindowsSwitched;
        public event Action ProgressRecovered;

        public void TryRecoveryProgress()
        {
            _inputService.SetActive(true);
            ProgressRecovered?.Invoke();
        }

        public void OnDefeat()
        {
            Defeated?.Invoke();
            _inputService.SetActive(false);
            StartTimer(callback: () => WindowsSwitched?.Invoke());
        }
    }
}
