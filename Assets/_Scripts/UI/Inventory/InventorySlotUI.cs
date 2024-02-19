using UnityEngine;

namespace RPG.UI.Inventory
{
    public class InventorySlotUI : MonoBehaviour, IDragContainer<Sprite>
    {

        [SerializeField]
        private InventoryItemIcon icon;

        public Sprite Item
        {
            get
            {
                return icon.GetItemSprite();
            }
        }

        public int ItemQuanity
        {
            get
            {
                return 1;
            }
        }

        public void AddItems(Sprite item, int quantity)
        {
            icon.SetItem(item);
        }

        public int GetMaxAcceptable(Sprite item)
        {
            if (Item == null)
            {
                return int.MaxValue;
            }
            return 0;
        }

        public void RemoveItems(int quantity)
        {
            icon.SetItem(null);
        }
    }
}
