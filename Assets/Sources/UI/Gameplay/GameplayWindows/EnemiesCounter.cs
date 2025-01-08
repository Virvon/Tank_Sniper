using Assets.Sources.Gameplay.Handlers;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Gameplay.GameplayWindows
{
    public class EnemiesCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _value;

        private WictoryHandler _winHandler;

        [Inject]
        private void Construct(WictoryHandler winHandler)
        {
            _winHandler = winHandler;

            ChangeValue(0);

            _winHandler.DestructedEnemiesCountChanger += ChangeValue;
        }

        private void OnDestroy() =>
            _winHandler.DestructedEnemiesCountChanger -= ChangeValue;

        private void ChangeValue(int destructedEnemiesCount) =>
            _value.text = $"{destructedEnemiesCount}/{_winHandler.MaxEnemiesCount}";
    }
}
