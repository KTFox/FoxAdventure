using UnityEngine;

namespace RPG.Inventory
{
    /// <summary>
    /// To be placed at the root of a Pickup prefab. Contains the data about the
    /// pickup such as the type of item and the number.
    /// </summary>
    public class Pickup : MonoBehaviour
    {
        private InventoryItemSO _item;
        private InventorySystem playerInventory;

        #region Properties
        public InventoryItemSO Item
        {
            get => _item;
        }

        public bool CanBePickedUp
        {
            get => playerInventory.HasEmptySlot;
        }
        #endregion

        private void Awake()
        {
            playerInventory = InventorySystem.PlayerInventory;
        }

        /// <summary>
        /// Set the vital data after creating the prefab
        /// </summary>
        /// <param name="item"></param>
        public void SetUp(in InventoryItemSO item)
        {
            _item = item;
        }

        public void PickupItem()
        {
            bool foundSlot = playerInventory.AddItemToFirstEmptySlot(_item);

            if (foundSlot)
            {
                Destroy(gameObject);
            }
        }
    }
}
