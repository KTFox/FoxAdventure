﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// A ScriptableObject that represents any _inventoryItem that can be put in an
    /// _inventory.
    /// </summary>
    /// <remarks>
    /// In practice, you are likely to use a subclass such as `ActionItemSO` or
    /// `EquipableItemSO`.
    /// </remarks>
    public abstract class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        // CONFIG DATA
        [Tooltip("Auto-generated UUID for saving/loading. Clear this field if you want to generate a new one.")]
        [SerializeField] string itemID = null;
        [Tooltip("InventoryItem name to be displayed in UI.")]
        [SerializeField] string displayName = null;
        [Tooltip("InventoryItem _description to be displayed in UI.")]
        [SerializeField][TextArea] string description = null;
        [Tooltip("The UI _inventoryItemIcon to represent this _inventoryItem in the _inventory.")]
        [SerializeField] Sprite icon = null;
        [Tooltip("The prefab that should be spawned when this _inventoryItem is dropped.")]
        [SerializeField] Pickup pickup = null;
        [Tooltip("If true, multiple items of this type can be stacked in the same _inventory slot.")]
        [SerializeField] bool stackable = false;

        // STATE
        static Dictionary<string, InventoryItem> itemLookupCache;

        // PUBLIC

        /// <summary>
        /// Get the _inventory _inventoryItem instance from its UUID.
        /// </summary>
        /// <param name="itemID">
        /// String UUID that persists between game instances.
        /// </param>
        /// <returns>
        /// Inventories _inventoryItem instance corresponding to the ID.
        /// </returns>
        public static InventoryItem GetFromID(string itemID)
        {
            if (itemLookupCache == null)
            {
                itemLookupCache = new Dictionary<string, InventoryItem>();
                var itemList = Resources.LoadAll<InventoryItem>("");
                foreach (var item in itemList)
                {
                    if (itemLookupCache.ContainsKey(item.itemID))
                    {
                        Debug.LogError(string.Format("Looks like there's a duplicate GameDevTV.UI.Inventories ID for objects: {0} and {1}", itemLookupCache[item.itemID], item));
                        continue;
                    }

                    itemLookupCache[item.itemID] = item;
                }
            }

            if (itemID == null || !itemLookupCache.ContainsKey(itemID)) return null;
            return itemLookupCache[itemID];
        }
        
        /// <summary>
        /// AttachWeaponToHand the pickup gameobject into the world.
        /// </summary>
        /// <param name="position">Where to spawn the pickup.</param>
        /// <param name="number">How many instances of the _inventoryItem does the pickup represent.</param>
        /// <returns>Reference to the pickup object spawned.</returns>
        public Pickup SpawnPickup(Vector3 position, int number)
        {
            var pickup = Instantiate(this.pickup);
            pickup.transform.position = position;
            pickup.Setup(this, number);
            return pickup;
        }

        public Sprite GetIcon()
        {
            return icon;
        }

        public string GetItemID()
        {
            return itemID;
        }

        public bool IsStackable()
        {
            return stackable;
        }
        
        public string GetDisplayName()
        {
            return displayName;
        }

        public string GetDescription()
        {
            return description;
        }

        // PRIVATE
        
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            // Generate and save a new UUID if this is blank.
            if (string.IsNullOrWhiteSpace(itemID))
            {
                itemID = System.Guid.NewGuid().ToString();
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            // Require by the ISerializationCallbackReceiver but we don't need
            // to do anything with it.
        }
    }
}
