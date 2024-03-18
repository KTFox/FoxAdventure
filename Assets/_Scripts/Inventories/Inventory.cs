using System;
using UnityEngine;
using System.Collections.Generic;
using RPG.Saving;

namespace RPG.Inventories
{
    public class Inventory : MonoBehaviour, ISaveable
    {
        // Structs

        [System.Serializable]
        private struct InventorySlotRecord
        {
            public string itemID;
            public int quantity;
        }

        private struct InventorySlot
        {
            public InventoryItemSO item;
            public int quantity;
        }

        // Variables

        [SerializeField]
        private int _inventorySize = 4;
        private InventorySlot[] slots;

        // Properties

        public static Inventory PlayerInventory
        {
            get
            {
                var player = GameObject.FindGameObjectWithTag("Player");

                return player.GetComponent<Inventory>();
            }
        }
        public int SlotSize => slots.Length;
        public bool HasEmptySlot => FindFirstEmptySlotIndex() >= 0;

        // Events

        public event Action OnInventoryUpdated;


        // Methods

        private void Awake()
        {
            slots = new InventorySlot[_inventorySize];
        }

        public bool AddItemToSlot(int slotIndex, InventoryItemSO item, int quantity)
        {
            if (slots[slotIndex].item != null)
            {
                return AddItemToFirstEmptySlot(item, quantity);
            }

            var i = FindStack(item);

            if (i >= 0)
            {
                slotIndex = i;
            }

            slots[slotIndex].item = item;
            slots[slotIndex].quantity += quantity;

            OnInventoryUpdated?.Invoke();

            return true;
        }

        public bool AddItemToFirstEmptySlot(InventoryItemSO item, int quantity)
        {
            foreach (var store in GetComponents<IItemStore>())
            {
                quantity -= store.AddItems(item, quantity);
            }

            if (quantity <= 0)
            {
                return true;
            }

            int index = FindSlot(item);

            if (index < 0)
            {
                return false;
            }

            slots[index].item = item;
            slots[index].quantity += quantity;

            OnInventoryUpdated?.Invoke();

            return true;
        }

        public void RemoveFromSlot(int index, int quantity)
        {
            slots[index].quantity -= quantity;

            if (slots[index].quantity <= 0)
            {
                slots[index].item = null;
                slots[index].quantity = 0;
            }

            OnInventoryUpdated?.Invoke();
        }

        public bool HasSpaceFor(IEnumerable<InventoryItemSO> items)
        {
            int freeSlotsNumber = GetFreeSlotsNumber();
            var stackedItems = new List<InventoryItemSO>();

            foreach (InventoryItemSO item in items)
            {
                if (item.Stackable)
                {
                    if (HasItem(item)) continue;
                    if (stackedItems.Contains(item)) continue;

                    stackedItems.Add(item);
                }

                if (freeSlotsNumber <= 0)
                {
                    return false;
                }

                freeSlotsNumber--;
            }

            return true;
        }

        public InventoryItemSO GetItemInSlot(int slotIndex)
        {
            return slots[slotIndex].item;
        }

        public int GetItemQuantityInSlot(int index)
        {
            return slots[index].quantity;
        }

        private int FindFirstEmptySlotIndex()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                {
                    return i;
                }
            }

            return -1;
        }

        private int FindSlot(InventoryItemSO item)
        {
            var i = FindStack(item);

            if (i < 0)
            {
                i = FindFirstEmptySlotIndex();
            }

            return i;
        }

        private int FindStack(InventoryItemSO item)
        {
            if (!item.Stackable)
            {
                return -1;
            }

            for (int i = 0; i < slots.Length; i++)
            {
                if (ReferenceEquals(slots[i].item, item))
                {
                    return i;
                }
            }

            return -1;
        }

        private int GetFreeSlotsNumber()
        {
            int count = 0;

            foreach (InventorySlot slot in slots)
            {
                if (slot.quantity == 0)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Is there an instance of _inventoryItem in the _inventory
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool HasItem(InventoryItemSO item)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (ReferenceEquals(slots[i].item, item))
                {
                    return true;
                }
            }

            return false;
        }

        #region ISaveable implements
        object ISaveable.CaptureState()
        {
            InventorySlotRecord[] slotRecordArray = new InventorySlotRecord[_inventorySize];

            for (int i = 0; i < _inventorySize; i++)
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

            for (int i = 0; i < _inventorySize; i++)
            {
                slots[i].item = InventoryItemSO.GetItemFromID(slotRecordArray[i].itemID);
                slots[i].quantity = slotRecordArray[i].quantity;
            }

            OnInventoryUpdated?.Invoke();
        }
        #endregion
    }
}
