using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPG.Inventories;

namespace RPG.UI.Inventories
{
    [RequireComponent(typeof(Image))]
    public class InventoryItemIcon : MonoBehaviour
    {
        [SerializeField]
        private GameObject itemQuantityTextContainer;
        [SerializeField]
        private TextMeshProUGUI itemQuantityText;

        public void SetItem(InventoryItemSO item)
        {
            SetItem(item, 0);
        }

        public void SetItem(InventoryItemSO item, int number)
        {
            var itemImage = GetComponent<Image>();

            if (item == null)
            {
                itemImage.enabled = false;
            }
            else
            {
                itemImage.enabled = true;
                itemImage.sprite = item.Icon;
            }

            if (itemQuantityText)
            {
                if (number <= 1)
                {
                    itemQuantityTextContainer.SetActive(false);
                }
                else
                {
                    itemQuantityTextContainer.SetActive(true);
                    itemQuantityText.text = number.ToString();
                }
            }
        }
    }
}
