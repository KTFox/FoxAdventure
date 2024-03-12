using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Inventories
{
    public class TabButtonGroup : MonoBehaviour
    {
        [SerializeField]
        private Color tabIdle;
        [SerializeField]
        private Color tabHover;
        [SerializeField]
        private Color tabActive;
        [SerializeField]
        private List<GameObject> tabsToSwap;

        private List<TabButton> tabButtons;
        private TabButton selectedTab;

        private void Start()
        {
            SubscribeAllButtons();
            OnTabSelected(tabButtons[0].GetComponent<TabButton>());
        }

        public void SubscribeAllButtons()
        {
            if (tabButtons == null)
            {
                tabButtons = new List<TabButton>();
            }

            foreach (TabButton tabButton in GetComponentsInChildren<TabButton>())
            {
                tabButtons.Add(tabButton);
            }
        }

        public void OnTabEnter(TabButton button)
        {
            ResetTabs();
            if (selectedTab == null || button != selectedTab)
            {
                button.SetBackground(tabHover);
            }
        }

        public void OnTabExit(TabButton button)
        {
            ResetTabs();
        }

        public void OnTabSelected(TabButton button)
        {
            selectedTab = button;
            ResetTabs();
            button.SetBackground(tabActive);

            int index = button.transform.GetSiblingIndex();
            for (int i = 0; i < tabsToSwap.Count; i++)
            {
                if (i == index)
                {
                    tabsToSwap[i].SetActive(true);
                }
                else
                {
                    tabsToSwap[i].SetActive(false);
                }
            }
        }

        public void ResetTabs()
        {
            foreach (TabButton button in tabButtons)
            {
                if (selectedTab != null && button == selectedTab) continue;
                button.SetBackground(tabIdle);
            }
        }
    }
}
