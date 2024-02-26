using System;
using UnityEngine;
using RPG.Saving;

namespace RPG.Inventory
{
    public class InventorySystem : MonoBehaviour, ISaveable
    {
        [SerializeField]
        private int inventorySize = 16;

        private InventoryItemSlot[] slots;

        #region Structs
        public struct InventoryItemSlot
        {
            public InventoryItemSO item;
            public int quantity;
        }
        #endregion

        public event Action OnInventoryUpdated;

        #region Properties
        public static InventorySystem PlayerInventory
        {
            get
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                return player.GetComponent<InventorySystem>();
            }
        }

        public int SlotSize
        {
            get => slots.Length;
        }

        public bool HasEmptySlot
        {
            get => FindFirstEmptySlotIndex() >= 0;
        }
        #endregion

        private void Awake()
        {
            slots = new InventoryItemSlot[inventorySize];
        }

        /// <summary>
        /// Will add an item to the given slot if possible.
        /// If there is already a stack of this type, it will be added into the exist stack. 
        /// Otherwise, it will be added into the first empty slot
        /// </summary>
        /// <param name="slotIndex"></param>
        /// <param name="item"></param>
        /// <returns>
        /// True if the item was added anywhere in the inventory
        /// </returns>
        public bool AddItemToSlot(int slotIndex, InventoryItemSO item, int number)
        {
            if (slots[slotIndex].item != null)
            {
                return AddItemToFirstEmptySlot(item, number);
            }

            var i = FindStack(item);
            if (i >= 0)
            {
                slotIndex = i;
            }

            slots[slotIndex].item = item;
            slots[slotIndex].quantity += number;

            OnInventoryUpdated?.Invoke();

            return true;
        }

        /// <summary>
        /// Attempt to add item into the first empty slot
        /// </summary>
        /// <param name="item"></param>
        /// <returns>
        /// false if inventory is full
        /// </returns>
        public bool AddItemToFirstEmptySlot(InventoryItemSO item, int number)
        {
            int index = FindSlot(item);

            if (index < 0)
                return false;

            slots[index].item = item;
            slots[index].quantity += number;

            OnInventoryUpdated?.Invoke();

            return true;
        }

        /// <summary>
        /// Remove the InventoryItemSO in given slot.
        /// Will never remove more that there are.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveFromSlot(int index, int number)
        {
            slots[index].quantity -= number;
            if (slots[index].quantity <= 0)
            {
                slots[index].item = null;
                slots[index].quantity = 0;
            }

            OnInventoryUpdated?.Invoke();
        }

        /// <summary>
        /// Get slotIndex of the first empty slot.
        /// </summary>
        /// <returns>
        /// -1 if inventory is full
        /// </returns>
        private int FindFirstEmptySlotIndex()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Get the InventoryItemSO in given slot
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public InventoryItemSO GetItemInSlot(int index)
        {
            return slots[index].item;
        }

        public int GetItemQuantityInSlot(int index)
        {
            return slots[index].quantity;
        }

        /// <summary>
        /// Is there an instance of item in the inventory
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool HasItem(InventoryItemSO item)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (ReferenceEquals(slots[i].item, item))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Find a slot that can accommodate the given item
        /// </summary>
        /// <param name="item"></param>
        /// <returns>returns -1 if slot is found</returns>
        private int FindSlot(InventoryItemSO item)
        {
            var i = FindStack(item);

            if (i < 0)
            {
                i = FindFirstEmptySlotIndex();
            }

            return i;
        }

        /// <summary>
        /// Find an existing stack of this item type.
        /// </summary>
        /// <returns>-1 if no stack exists or if the item is not stackable.</returns>
        private int FindStack(InventoryItemSO item)
        {
            if (!item.Stackable)
                return -1;

            for (int i = 0; i < slots.Length; i++)
            {
                if (ReferenceEquals(slots[i].item, item))
                {
                    return i;
                }
            }

            return -1;
        }

        #region ISaveable implements
        [System.Serializable]
        private struct InventorySlotRecord
        {
            public string itemID;
            public int quantity;
        }

        object ISaveable.CaptureState()
        {
            InventorySlotRecord[] slotRecordArray = new InventorySlotRecord[inventorySize];

            for (int i = 0; i < inventorySize; i++)
            {
                if (slots[i].item != null)
                {
                    slotRecordArray[i].itemID = slots[i].item.ItemID;
                    slotRecordArray[i].quantity = slots[i].quantity;
                }
            }

            return slotRecordArray;
        }

        void ISaveable.RestoreState(object state)
        {
            InventorySlotRecord[] slotRecordArray = (InventorySlotRecord[])state;

            for (int i = 0; i < inventorySize; i++)
            {
                slots[i].item = InventoryItemSO.GetItemFromID(slotRecordArray[i].itemID);
                slots[i].quantity = slotRecordArray[i].quantity;
            }

            OnInventoryUpdated?.Invoke();
        }
        #endregion
    }
}
