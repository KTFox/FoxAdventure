using System;
using UnityEngine;
using System.Collections.Generic;
using RPG.Saving;

namespace RPG.Inventories
{
    /// <summary>
    /// Provides a store for the items equipped to a player. 
    /// Items are stored by their equip locations.
    /// This component should be placed on the GameObject tagged "Player".
    /// </summary>
    public class Equipment : MonoBehaviour, ISaveable
    {
        public event Action OnEquipmentUpdated;

        private Dictionary<EquipLocation, EquipableItemSO> equippedItems = new Dictionary<EquipLocation, EquipableItemSO>();

        /// <summary>
        /// Return the _item in the given equip location.
        /// </summary>
        public EquipableItemSO GetItemInSlot(EquipLocation location)
        {
            if (!equippedItems.ContainsKey(location))
            {
                return null;
            }

            return equippedItems[location];
        }

        /// <summary>
        /// Add an _item to the given equip location. 
        /// Do not attempt to equip to an incompatible slot.
        /// </summary>
        public void AddItem(EquipLocation location, EquipableItemSO item)
        {
            equippedItems[location] = item;

            OnEquipmentUpdated?.Invoke();
        }

        /// <summary>
        /// Remove the _item for the given location.
        /// </summary>
        /// <param name="location"></param>
        public void RemoveItem(EquipLocation location)
        {
            equippedItems.Remove(location);

            OnEquipmentUpdated?.Invoke();
        }

        /// <summary>
        /// Enumerate through all the slots that currently contains _item.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EquipLocation> GetAllPopulatedSlots()
        {
            return equippedItems.Keys;
        }

        #region ISaveable implements
        object ISaveable.CaptureState()
        {
            var equippedItemsForSerialization = new Dictionary<EquipLocation, string>();
            foreach (var pair in equippedItems)
            {
                equippedItemsForSerialization[pair.Key] = pair.Value.ItemID;
            }

            return equippedItemsForSerialization;
        }

        void ISaveable.RestoreState(object state)
        {
            equippedItems = new Dictionary<EquipLocation, EquipableItemSO>();

            var equippedItemsForSerialization = (Dictionary<EquipLocation, string>)state;
            foreach (var pair in equippedItemsForSerialization)
            {
                var item = (EquipableItemSO)InventoryItemSO.GetItemFromID(pair.Value);

                if (item != null)
                {
                    equippedItems[pair.Key] = item;
                }
            }

            OnEquipmentUpdated?.Invoke();
        }
        #endregion
    }
}
