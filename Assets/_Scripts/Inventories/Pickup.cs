using UnityEngine;

namespace RPG.Inventories
{
    /// <summary>
    /// To be placed at the root of a Pickup prefab. Contains the data about the
    /// pickup such as the type of _item and the quantity.
    /// </summary>
    public class Pickup : MonoBehaviour
    {
        private InventoryItemSO _item;
        private Inventory playerInventory;

        private int _number = 1;

        #region Properties
        public InventoryItemSO Item => _item;
        public bool CanBePickedUp => playerInventory.HasEmptySlot;
        public int Number => _number;
        #endregion

        private void Awake()
        {
            playerInventory = Inventory.PlayerInventory;
        }

        /// <summary>
        /// Set the vital data after creating the prefab
        /// </summary>
        /// <param name="item"></param>
        public void SetUp(in InventoryItemSO item, int number)
        {
            _item = item;

            if (!_item.Stackable)
            {
                number = 1;
            }

            _number = number;

        }

        public void PickupItem()
        {
            bool foundSlot = playerInventory.AddItemToFirstEmptySlot(_item, _number);

            if (foundSlot)
            {
                Destroy(gameObject);
            }
        }
    }
}
