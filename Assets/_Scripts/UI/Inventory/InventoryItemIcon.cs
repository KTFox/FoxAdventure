using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Inventory {
    [RequireComponent(typeof(Sprite))]
    public class InventoryItemIcon : MonoBehaviour {
        public void SetItem(Sprite item)
        {
            var itemImage = GetComponent<Image>();

            if (item == null)
            {
                itemImage.enabled = false;
            }
            else
            {
                itemImage.enabled = true;
                itemImage.sprite = item;
            }
        }

        public Sprite GetItemSprite()
        {
            var itemImage = GetComponent<Image>();

            if (!itemImage.enabled)
            {
                return null;
            }
            else
            {
                return itemImage.sprite;
            }
        }
    }
}
