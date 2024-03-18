using System;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Inventories
{
    public class ActionStore : MonoBehaviour, ISaveable
    {
        // Structs

        private class DockedItemSlot
        {
            public ActionItemSO item;
            public int quantity;
        }

        [System.Serializable]
        private struct DockedItemRecord
        {
            public string itemID;
            public int quantity;
        }

        // Variables

        private Dictionary<int, DockedItemSlot> dockedItems = new Dictionary<int, DockedItemSlot>();

        // Events

        public event Action OnActionStoreUpdated;


        // Methods

        public bool UseActionItem(int slotIndex, GameObject user)
        {
            if (dockedItems.ContainsKey(slotIndex))
            {
                bool wasUsed = dockedItems[slotIndex].item.UseActionItem(user);

                if (wasUsed && dockedItems[slotIndex].item.Consumable)
                {
                    RemoveActionItem(slotIndex, 1);
                }

                return true;
            }

            return false;
        }

        public void AddActionItem(InventoryItemSO item, int slotIndex, int quantity)
        {
            if (dockedItems.ContainsKey(slotIndex))
            {
                if (ReferenceEquals(dockedItems[slotIndex], (ActionItemSO)item))
                {
                    dockedItems[slotIndex].quantity += quantity;
                }
            }
            else
            {
                var dockedItemSlot = new DockedItemSlot();
                dockedItemSlot.item = (ActionItemSO)(item);
                dockedItemSlot.quantity = quantity;

                dockedItems[slotIndex] = dockedItemSlot;
            }

            OnActionStoreUpdated?.Invoke();
        }

        public void RemoveActionItem(int slotIndex, int quantity)
        {
            if (dockedItems.ContainsKey(slotIndex))
            {
                dockedItems[slotIndex].quantity -= quantity;

                if (dockedItems[slotIndex].quantity <= 0)
                {
                    dockedItems.Remove(slotIndex);
                }

                OnActionStoreUpdated?.Invoke();
            }
        }

        public ActionItemSO GetActionItem(int slotIndex)
        {
            if (dockedItems.ContainsKey(slotIndex))
            {
                return dockedItems[slotIndex].item;
            }

            return null;
        }

        public int GetItemQuantity(int slotIndex)
        {
            if (dockedItems.ContainsKey(slotIndex))
            {
                return dockedItems[slotIndex].quantity;
            }

            return 0;
        }

        /// <summary>
        /// What is the maximum quantity of items allowed in this slot.
        /// This takes into account whether the slot already contains an _inventoryItem and whether it is the same type.
        /// Will only accept multiple if the _inventoryItem is consumable.
        /// </summary>
        /// <returns>Will return int.MaxValue when there is not effective bound.</returns>
        public int GetMaxAcceptable(InventoryItemSO item, int slotIndex)
        {
            var actionItem = item as ActionItemSO;

            if (!actionItem)
            {
                return 0;
            }

            /* 
            1. Slot is empty:
                - Input _inventoryItem is consumable => int.maxvalue
                - Input _inventoryItem isn't consumable => 1
            2. Slot already has had _inventoryItem:
                - Two items are similar
                    + Input _inventoryItem is consumable => int.maxValue
                    + Input _inventoryItem isn't consumable => 0
                - Two items are different => 0
            */

            if (dockedItems.ContainsKey(slotIndex) && !ReferenceEquals(item, dockedItems[slotIndex].item))
            {
                return 0;
            }

            if (actionItem.Consumable)
            {
                return int.MaxValue;
            }

            if (dockedItems.ContainsKey(slotIndex))
            {
                return 0;
            }

            return 1;
        }

        #region ISaveable implements
        object ISaveable.CaptureState()
        {
            var state = new Dictionary<int, DockedItemRecord>();

            foreach (var pair in dockedItems)
            {
                var record = new DockedItemRecord();
                record.itemID = pair.Value.item.ItemID;
                record.quantity = pair.Value.quantity;

                state[pair.Key] = record;
            }

            return state;
        }

        void ISaveable.RestoreState(object state)
        {
            var stateForRestore = (Dictionary<int, DockedItemRecord>)state;

            foreach (var pair in stateForRestore)
            {
                AddActionItem(InventoryItemSO.GetItemFromID(pair.Value.itemID), pair.Key, pair.Value.quantity);
            }
        }
        #endregion
    }
}
