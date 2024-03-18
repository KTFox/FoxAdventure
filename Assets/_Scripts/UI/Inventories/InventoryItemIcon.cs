using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPG.Inventories;

namespace RPG.UI.Inventories
{
    [RequireComponent(typeof(Image))]
    public class InventoryItemIcon : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private GameObject _itemQuantityTextContainer;
        [SerializeField]
        private TextMeshProUGUI _itemQuantityText;


        // Methods

        public void SetItem(InventoryItemSO inventoryItemSO)
        {
            SetItem(inventoryItemSO, 0);
        }

        public void SetItem(InventoryItemSO inventoryItemSO, int quantity)
        {
            var itemImage = GetComponent<Image>();

            if (inventoryItemSO == null)
            {
                itemImage.enabled = false;
            }
            else
            {
                itemImage.enabled = true;
                itemImage.sprite = inventoryItemSO.Icon;
            }

            if (_itemQuantityText)
            {
                if (quantity <= 1)
                {
                    _itemQuantityTextContainer.SetActive(false);
                }
                else
                {
                    _itemQuantityTextContainer.SetActive(true);
                    _itemQuantityText.text = quantity.ToString();
                }
            }
        }
    }
}
