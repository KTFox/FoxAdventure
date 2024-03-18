using UnityEngine;
using RPG.Inventories;
using RPG.Utility.UI;

namespace RPG.UI.Inventories
{
    public class InventorySlotUI : MonoBehaviour, IDragContainer<InventoryItemSO>, IItemHolder
    {
        // Variables

        [SerializeField]
        private InventoryItemIcon _inventoryItemIcon;

        private int _slotIndex;
        private Inventory _inventory;

        // Properties

        public InventoryItemSO InventoryItemSO => _inventory.GetItemInSlot(_slotIndex);
        public int ItemQuantity => _inventory.GetItemQuantityInSlot(_slotIndex);
        

        // Methods

        public void SetUp(Inventory inventory, int slotIndex)
        {
            _inventory = inventory;
            _slotIndex = slotIndex;
            _inventoryItemIcon.SetItem(inventory.GetItemInSlot(slotIndex), inventory.GetItemQuantityInSlot(slotIndex));
        }

        public void AddItems(InventoryItemSO inventoryItemSO, int quantity)
        {
            _inventory.AddItemToSlot(_slotIndex, inventoryItemSO, quantity);
        }

        public void RemoveItems(int quantity)
        {
            _inventory.RemoveFromSlot(_slotIndex, quantity);
        }

        public int GetMaxAcceptable(InventoryItemSO inventoryItemSO)
        {
            if (InventoryItemSO == null)
            {
                return int.MaxValue;
            }

            return 0;
        }
    }
}
