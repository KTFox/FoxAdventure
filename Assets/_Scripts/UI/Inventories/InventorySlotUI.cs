using UnityEngine;
using RPG.Inventories;
using RPG.Utility.UI;

namespace RPG.UI.Inventories
{
    public class InventorySlotUI : MonoBehaviour, IDragContainer<InventoryItemSO>, IItemHolder
    {
        #region Variables
        [SerializeField]
        private InventoryItemIcon icon;

        private int index;
        private RPG.Inventories.Inventory inventory;
        #endregion

        #region Properties
        public InventoryItemSO Item => inventory.GetItemInSlot(index);
        public int ItemQuantity => inventory.GetItemQuantityInSlot(index);
        #endregion

        public void SetUp(Inventory inventory, int index)
        {
            this.inventory = inventory;
            this.index = index;
            icon.SetItem(inventory.GetItemInSlot(index), inventory.GetItemQuantityInSlot(index));
        }

        public void AddItems(InventoryItemSO item, int number)
        {
            inventory.AddItemToSlot(index, item, number);
        }

        public void RemoveItems(int quantity)
        {
            inventory.RemoveFromSlot(index, quantity);
        }

        public int GetMaxAcceptable(InventoryItemSO item)
        {
            if (Item == null)
            {
                return int.MaxValue;
            }
            return 0;
        }
    }
}
