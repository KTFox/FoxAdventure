using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.UI.Inventories
{
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        private TabButtonGroup _tabButtonGroup;
        private Image _background;

        private void Awake()
        {
            _background = GetComponent<Image>();
            _tabButtonGroup = GetComponentInParent<TabButtonGroup>();
        }

        #region IPointerHandler implements
        public void OnPointerEnter(PointerEventData eventData)
        {
            _tabButtonGroup.OnPointerEnterTab(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _tabButtonGroup.OnTabSelected(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _tabButtonGroup.OnPointerExitTab(this);
        }
        #endregion

        public void SetBackground(Color color)
        {
            _background.color = color;
        }
    }
}
