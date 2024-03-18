using UnityEngine;

namespace RPG.Inventories
{
    public class Pickup : MonoBehaviour
    {
        // Variables

        private InventoryItemSO _inventoryItem;
        private Inventory _playerInventory;
        private int _quantity = 1;

        // Properties

        public InventoryItemSO Item => _inventoryItem;
        public bool CanBePickedUp => _playerInventory.HasEmptySlot;
        public int Quantity => _quantity;
        

        // Methods

        private void Awake()
        {
            _playerInventory = Inventory.PlayerInventory;
        }

        public void SetUp(in InventoryItemSO item, int quantity)
        {
            _inventoryItem = item;

            if (!_inventoryItem.Stackable)
            {
                quantity = 1;
            }

            _quantity = quantity;

        }

        public void PickupItem()
        {
            bool foundSlot = _playerInventory.AddItemToFirstEmptySlot(_inventoryItem, _quantity);

            if (foundSlot)
            {
                Destroy(gameObject);
            }
        }
    }
}
