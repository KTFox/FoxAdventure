using UnityEngine;
using UnityEngine.UI;
using RPG.Inventory;

namespace RPG.UI.Inventory
{
    [RequireComponent(typeof(Image))]
    public class InventoryItemIcon : MonoBehaviour
    {
        /// <summary>
        /// Set item image
        /// </summary>
        /// <param name="item"></param>
        public void SetItem(InventoryItemSO item)
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
        }
    }
}
