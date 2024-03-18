using System;
using UnityEngine;
using System.Collections.Generic;
using RPG.Saving;

namespace RPG.Inventories
{
    public class Equipment : MonoBehaviour, ISaveable
    {
        // Variables

        private Dictionary<EquipLocation, EquipableItemSO> equippedItems = new Dictionary<EquipLocation, EquipableItemSO>();

        // Events

        public event Action OnEquipmentUpdated;


        // Methods

        public EquipableItemSO GetItemInSlot(EquipLocation location)
        {
            if (!equippedItems.ContainsKey(location))
            {
                return null;
            }

            return equippedItems[location];
        }

        public void AddItem(EquipLocation location, EquipableItemSO item)
        {
            equippedItems[location] = item;

            OnEquipmentUpdated?.Invoke();
        }

        public void RemoveItem(EquipLocation location)
        {
            equippedItems.Remove(location);

            OnEquipmentUpdated?.Invoke();
        }

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
