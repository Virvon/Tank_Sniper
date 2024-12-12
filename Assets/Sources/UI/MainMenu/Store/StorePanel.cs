using TMPro;
using UnityEngine;

namespace Assets.Sources.UI.MainMenu.Store
{
    public class StorePanel : MonoBehaviour
    {
        [SerializeField] private StorePanelTab[] _tabs;
        [SerializeField] private StorePanelTab _startTab;
        [SerializeField] private TMP_Text _tabNameValue;

        private StorePanelTab _currentTab;

        private void OnEnable()
        {
            foreach (StorePanelTab tab in _tabs)
            {
                tab.Hide();
                tab.Choosed += ChangeTab;
            }

            ChangeTab(_startTab);
        }

        private void OnDisable()
        {
            foreach (StorePanelTab tab in _tabs)
                tab.Choosed -= ChangeTab;
        }

        private void ChangeTab(StorePanelTab tab)
        {
            _currentTab?.Hide();
            _currentTab = tab;
            _currentTab.Open();

            _tabNameValue.text = _currentTab.Name;
        }
    }
}
