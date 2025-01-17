using TMPro;
using UnityEngine;

namespace Assets.Sources.UI.Gameplay.WictoryWindow
{
    public class RouletteWictoryWindow : WicotoryWindow
    {
        private const string RewardInfo = "ДОСТАТОЧНО";

        [SerializeField] private Roulette _roulette;
        [SerializeField] private TMP_Text _continueButtonRewardValue;

        protected override void Start()
        {
            base.Start();
            _roulette.Rewarded += OnRouletteRewarded;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _roulette.Rewarded -= OnRouletteRewarded;
        }

        private void OnRouletteRewarded(uint reward)
        {
            PersistentProgressService.Progress.Wallet.Give(reward);
            LoadNextScene();
        }

        protected override void OnWindowsSwitched()
        {
            uint reward = RewardCounter.GetReward();
            _continueButtonRewardValue.text = $"{reward} {RewardInfo}";
            _roulette.Work(reward);

            base.OnWindowsSwitched();
        }
    }
}
