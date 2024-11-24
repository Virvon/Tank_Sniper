using Assets.Sources.Utils;
using MPUIKIT;
using UnityEngine;

namespace Assets.Sources.Ui
{
    public class Healthbar : MonoBehaviour
    {
        [SerializeField] private MPImage _barFill;
        [SerializeField] private MPImage _heartFill;
        [SerializeField, Range(0, 1)] private float _fillAmount;
        [SerializeField, Range(0, 1)] private float _barFillPercent;

        private void OnValidate()
        {
            _barFill.fillAmount = Extensions.Remap(_fillAmount, 1 - _barFillPercent, 1, 0, 1);
            _heartFill.fillAmount = Extensions.Remap(_fillAmount, 0, 1 - _barFillPercent, 0, 1);
        }
    }
}
