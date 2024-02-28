using System;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Inventory
{
    /// <summary>
    /// Provides the storage for an action bar. The bar has a finite quantity of slots that can be filled and actions in the slots can be "used".
    /// This component should be placed on the GameObject tagged "Player".
    public class ActionStore : MonoBehaviour, ISaveable
    {
        public event Action OnActionStoreUpdate;

        private Dictionary<int, DockedItemSlot> dockedItems = new Dictionary<int, DockedItemSlot>();

        private class DockedItemSlot
        {
            public ActionItemSO item;
            public int quantity;
        }

        /// <summary>
        /// Add an action to the given index
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <param name="quantity"></param>
        public void AddActionItem(InventoryItemSO item, int index, int quantity)
        {
            if (dockedItems.ContainsKey(index))
            {
                if (ReferenceEquals(dockedItems[index], (ActionItemSO)item))
                {
                    dockedItems[index].quantity += quantity;
                }
                else
                {
                    var dockedItemSlot = new DockedItemSlot();
                    dockedItemSlot.item = (ActionItemSO)(item);
                    dockedItemSlot.quantity = quantity;

                    dockedItems[index] = dockedItemSlot;
                }
            }

            OnActionStoreUpdate?.Invoke();
        }

        /// <summary>
        /// Use the item at the given slot. 
        /// If the item is consumable one instance will be destroyed until the item is removed completely.
        /// </summary>
        /// <param name="user">The character that wants to use this action.</param>
        /// <returns>False if the action could not be executed.</returns>
        public bool UseActionItem(int index, GameObject user)
        {
            if (dockedItems.ContainsKey(index))
            {
                dockedItems[index].item.Use(user);

                if (dockedItems[index].item.Consumable)
                {
                    RemoveActionItem(index, 1);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Remove the given quantity of the items at the given slot.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="quantity"></param>
        public void RemoveActionItem(int index, int quantity)
        {
            if (dockedItems.ContainsKey(index))
            {
                dockedItems[index].quantity -= quantity;

                if (dockedItems[index].quantity <= 0)
                {
                    dockedItems.Remove(index);
                }

                OnActionStoreUpdate?.Invoke();
            }
        }

        /// <summary>
        /// Get the action at the given index
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
        /// Get the quantity of items left at the given index.
        /// </summary>
        /// <returns>
        /// Will return 0 if no item is in the index or the item has been fully consumed.
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

            if (dockedItems.ContainsKey(index) && !object.ReferenceEquals(item, dockedItems[index].item))
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
