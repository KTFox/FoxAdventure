using System;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Inventories
{
    /// <summary>
    /// Contain information about items in the ActionBar".
    /// This component should be placed on the GameObject tagged "Player".
    public class ActionStore : MonoBehaviour, ISaveable
    {
        public event Action OnActionStoreUpdated;

        private Dictionary<int, DockedItemSlot> dockedItems = new Dictionary<int, DockedItemSlot>();

        private class DockedItemSlot
        {
            public ActionItemSO item;
            public int quantity;
        }

        /// <summary>
        /// Add the given quantity of the items into the given slot.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="slotIndex"></param>
        /// <param name="quantity"></param>
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

        /// <summary>
        /// Remove the given quantity of the items at the given slot.
        /// </summary>
        /// <param name="slotIndex"></param>
        /// <param name="quantity"></param>
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

        /// <summary>
        /// Use the item at the given slot. 
        /// If the item is consumable, one instance will be destroyed until the item is removed completely.
        /// </summary>
        /// <param name="user">The character that wants to use this action.</param>
        /// <returns>False if the action could not be executed.</returns>
        public bool UseActionItem(int index, GameObject user)
        {
            if (dockedItems.ContainsKey(index))
            {
                bool wasUsed = dockedItems[index].item.Use(user);
                if (wasUsed && dockedItems[index].item.Consumable)
                {
                    RemoveActionItem(index, 1);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Get the action item at the given slotIndex
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ActionItemSO GetActionItem(int index)
        {
            if (dockedItems.ContainsKey(index))
            {
                return dockedItems[index].item;
            }

            return null;
        }

        /// <summary>
        /// Get the quantity of items left at the given slotIndex.
        /// </summary>
        /// <returns>
        /// Will return 0 if no item is in the slotIndex or the item has been fully consumed.
        /// </returns>
        public int GetItemQuantity(int index)
        {
            if (dockedItems.ContainsKey(index))
            {
                return dockedItems[index].quantity;
            }

            return 0;
        }

        /// <summary>
        /// What is the maximum quantity of items allowed in this slot.
        /// This takes into account whether the slot already contains an item and whether it is the same type.
        /// Will only accept multiple if the item is consumable.
        /// </summary>
        /// <returns>Will return int.MaxValue when there is not effective bound.</returns>
        public int GetMaxAcceptable(InventoryItemSO item, int index)
        {
            var actionItem = item as ActionItemSO;
            if (!actionItem) return 0;

            /* 
            1. Slot is empty:
                - Input item is consumable => int.maxvalue
                - Input item isn't consumable => 1
            2. Slot already has had item:
                - Two items are similar
                    + Input item is consumable => int.maxValue
                    + Input item isn't consumable => 0
                - Two items are different => 0
            */

            if (dockedItems.ContainsKey(index) && !ReferenceEquals(item, dockedItems[index].item))
            {
                return 0;
            }

            if (actionItem.Consumable)
            {
                return int.MaxValue;
            }

            if (dockedItems.ContainsKey(index))
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

        [System.Serializable]
        private struct DockedItemRecord
        {
            public string itemID;
            public int quantity;
        }
        #endregion
    }
}
