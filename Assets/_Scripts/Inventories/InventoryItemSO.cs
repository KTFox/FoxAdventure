using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    /// <summary>
    /// A ScriptableObject that represents any _inventoryItem that can be put in an _inventory
    /// </summary>
    /// <remarks>
    /// In practice, you are likely to use a subclass such as "ActionItemSO" or "EquipableItemSO"
    /// </remarks>
    public class InventoryItemSO : ScriptableObject, ISerializationCallbackReceiver
    {
        // Variables

        [Tooltip("Auto-generated UUID for saving/loading. Clear this field if you want to generate a new one.")]
        [SerializeField]
        private string _itemID;

        [Tooltip("InventoryItem name to be displayed in UI.")]
        [SerializeField]
        private string _displayName;

        [Tooltip("InventoryItem _description to be displayed in UI.")]
        [SerializeField]
        [TextArea]
        private string _description;

        [Tooltip("The UI _inventoryItemIcon to represent this _inventoryItem in the _inventory.")]
        [SerializeField]
        private Sprite _icon;

        [Tooltip("The prefab that should be spawned when this _inventoryItem is dropped.")]
        [SerializeField]
        private Pickup pickup;

        [Tooltip("If true, multiple items of this type can be stacked in the same _inventory slot.")]
        [SerializeField]
        private bool _stackable;

        [Tooltip("InventoryItem's buying price in shop.")]
        [SerializeField]
        private float _price;

        [SerializeField]
        private ItemCategory _itemCategory;

        private static Dictionary<string, InventoryItemSO> itemLookupTable;

        // Properties

        public string ItemID => _itemID;
        public string DisplayName => _displayName;
        public string Description => _description;
        public Sprite Icon => _icon;
        public bool Stackable => _stackable;
        public float Price => _price;
        public ItemCategory ItemCategory => _itemCategory;


        // Methods

        /// <summary>
        /// Get the _inventory _inventoryItem from its ID
        /// </summary>
        /// <param name="itemID">
        /// String UUID that persists between game instances.
        /// </param>
        /// <returns></returns>
        public static InventoryItemSO GetItemFromID(string itemID)
        {
            if (itemLookupTable == null)
            {
                itemLookupTable = new Dictionary<string, InventoryItemSO>();
                var itemList = Resources.LoadAll<InventoryItemSO>("");

                foreach (var item in itemList)
                {
                    if (itemLookupTable.ContainsKey(item._itemID))
                    {
                        Debug.LogError($"It looks like that InventoryItemSO({item._itemID}) in Resources folder has been duplicated");
                        continue;
                    }

                    itemLookupTable[item._itemID] = item;
                }
            }

            if (itemID == null || !itemLookupTable.ContainsKey(itemID))
            {
                return null;
            }

            return itemLookupTable[itemID];
        }

        /// <summary>
        /// AttachWeaponToHand pickup into the world
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Pickup SpawnPickup(Vector3 position, int quantity)
        {
            var pickup = Instantiate(this.pickup);
            pickup.transform.position = position;
            pickup.SetUp(this, quantity);

            return pickup;
        }

        #region ISerializationCallbackReceiver implements
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            // Generate new UUID if this is blank
            if (string.IsNullOrWhiteSpace(_itemID))
            {
                _itemID = Guid.NewGuid().ToString();
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            // Don't need to do anything
        }
        #endregion
    }
}