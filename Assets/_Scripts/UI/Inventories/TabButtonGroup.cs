using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Inventories
{
    public class TabButtonGroup : MonoBehaviour
    {
        [SerializeField]
        private Color _idleColor;
        [SerializeField]
        private Color _hoverColor;
        [SerializeField]
        private Color _activeColor;
        [SerializeField]
        private List<GameObject> _tabsToSwap;

        private List<TabButton> _tabButtons;
        private TabButton _currentSelectedTab;

        private void Start()
        {
            SubscribeAllTabButtons();
            OnTabSelected(_tabButtons[0].GetComponent<TabButton>());
        }

        public void SubscribeAllTabButtons()
        {
            if (_tabButtons == null)
            {
                _tabButtons = new List<TabButton>();
            }

            foreach (var tabButton in GetComponentsInChildren<TabButton>())
            {
                _tabButtons.Add(tabButton);
            }
        }

        public void OnPointerEnterTab(TabButton tabButton)
        {
            ResetTabButtons();

            if (_currentSelectedTab == null || tabButton != _currentSelectedTab)
            {
                tabButton.SetBackground(_hoverColor);
            }
        }

        public void OnPointerExitTab(TabButton tabButton)
        {
            ResetTabButtons();
        }

        public void OnTabSelected(TabButton tabButton)
        {
            _currentSelectedTab = tabButton;
            ResetTabButtons();
            tabButton.SetBackground(_activeColor);

            int tabButtonIndex = tabButton.transform.GetSiblingIndex();

            for (int i = 0; i < _tabsToSwap.Count; i++)
            {
                if (i == tabButtonIndex)
                {
                    _tabsToSwap[i].SetActive(true);
                }
                else
                {
                    _tabsToSwap[i].SetActive(false);
                }
            }
        }

        public void ResetTabButtons()
        {
            foreach (var button in _tabButtons)
            {
                if (_currentSelectedTab != null && button == _currentSelectedTab) continue;

                button.SetBackground(_idleColor);
            }
        }
    }
}
