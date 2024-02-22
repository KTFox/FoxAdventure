using System;
using UnityEngine;
using RPG.Saving;

namespace RPG.Inventory
{
    public class InventorySystem : MonoBehaviour, ISaveable
    {
        [SerializeField]
        private int inventorySize = 16;

        private InventoryItemSO[] slots;

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
            get => FindFirstEmptySlotIndex() < 0;
        }
        #endregion

        private void Awake()
        {
            slots = new InventoryItemSO[inventorySize];
            slots[0] = InventoryItemSO.GetItemFromID("19eef2b6-954e-4b79-807d-44c4ac72a220");
            slots[1] = InventoryItemSO.GetItemFromID("31377e4f-a469-4f5d-b166-a52a11722c9a");
        }

        /// <summary>
        /// Will add an item to the given slot if possible.
        /// If there is already a stack of this type, it will be added into the exist stack. 
        /// Otherwise, it will be added into the first empty slot
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <returns>
        /// True if the item was added anywhere in the inventory
        /// </returns>
        public bool AddItemToSlot(int index, InventoryItemSO item)
        {
            if (slots[index] != null)
            {
                return AddItemToFirstEmptySlot(item);
            }

            slots[index] = item;
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
        public bool AddItemToFirstEmptySlot(InventoryItemSO item)
        {
            int index = FindFirstEmptySlotIndex();

            if (index < 0)
                return false;

            slots[index] = item;
            OnInventoryUpdated?.Invoke();

            return true;
        }

        /// <summary>
        /// Remove the InventoryItemSO in given slot
        /// </summary>
        /// <param name="index"></param>
        public void RemoveFromSlot(int index)
        {
            slots[index] = null;
            OnInventoryUpdated?.Invoke();
        }

        /// <summary>
        /// Get index of the first empty slot.
        /// </summary>
        /// <returns>
        /// -1 if inventory is full
        /// </returns>
        private int FindFirstEmptySlotIndex()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null)
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
            return slots[index];
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
                if (ReferenceEquals(slots[i], item))
                    return true;
            }

            return false;
        }

        #region ISaveable implements
        object ISaveable.CaptureState()
        {
            string[] slotIDList = new string[inventorySize];

            for (int i = 0; i < inventorySize; i++)
            {
                if (slots[i] != null)
                {
                    slotIDList[i] = slots[i].ItemID;
                }
            }

            return slotIDList;
        }

        void ISaveable.RestoreState(object state)
        {
            string[] slotIDList = (string[])state;

            for (int i = 0; i < inventorySize; i++)
            {
                slots[i] = InventoryItemSO.GetItemFromID(slotIDList[i]);
            }

            OnInventoryUpdated?.Invoke();
        }
        #endregion
    }
}
