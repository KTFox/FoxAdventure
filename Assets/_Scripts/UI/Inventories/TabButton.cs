using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.UI.Inventories
{
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        private TabButtonGroup tabButtonGroup;
        private Image background;

        private void Awake()
        {
            background = GetComponent<Image>();
            tabButtonGroup = GetComponentInParent<TabButtonGroup>();
        }

        #region IPointerHandler implements
        public void OnPointerEnter(PointerEventData eventData)
        {
            tabButtonGroup.OnTabEnter(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            tabButtonGroup.OnTabSelected(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tabButtonGroup.OnTabExit(this);
        }
        #endregion

        public void SetBackground(Color buttonColor)
        {
            background.color = buttonColor;
        }
    }
}
