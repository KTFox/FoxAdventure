using UnityEngine;
using RPG.Inventory;

namespace RPG.UI.Inventory
{
    public class InventorySlotUI : MonoBehaviour, IDragContainer<InventoryItemSO>
    {
        #region Variables
        [SerializeField]
        private InventoryItemIcon icon;

        private int index;
        private InventorySystem inventory;
        #endregion

        #region Properties
        public InventoryItemSO Item
        {
            get => inventory.GetItemInSlot(index);
        }

        public int ItemQuanity
        {
            get => 1;
        }
        #endregion

        public void SetUp(InventorySystem inventory, int index)
        {
            this.inventory = inventory;
            this.index = index;
            icon.SetItem(inventory.GetItemInSlot(index));
        }

        public void AddItems(InventoryItemSO item, int number)
        {
            inventory.AddItemToSlot(index, item);
        }

        public void RemoveItems(int quantity)
        {
            inventory.RemoveFromSlot(index);
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
