using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    /// <summary>
    /// A ScriptableObject that represents any _item that can be put in an inventory
    /// </summary>
    /// <remarks>
    /// In practice, you are likely to use a subclass such as "ActionItemSO" or "EquipableItemSO"
    /// </remarks>
    public class InventoryItemSO : ScriptableObject, ISerializationCallbackReceiver
    {
        #region Variables
        [Tooltip("Auto-generated UUID for saving/loading. Clear this field if you want to generate a new one.")]
        [SerializeField]
        private string _itemID;

        [Tooltip("Item name to be displayed in UI.")]
        [SerializeField]
        private string _displayName;

        [Tooltip("Item description to be displayed in UI.")]
        [SerializeField]
        [TextArea]
        private string _description;

        [Tooltip("The UI icon to represent this _item in the inventory.")]
        [SerializeField]
        private Sprite _icon;

        [Tooltip("The prefab that should be spawned when this _item is dropped.")]
        [SerializeField]
        private Pickup pickup;

        [Tooltip("If true, multiple items of this type can be stacked in the same inventory slot.")]
        [SerializeField]
        private bool _stackable;

        [Tooltip("Item's buying price in shop. ")]
        [SerializeField]
        private float _price;

        [SerializeField]
        private ItemCategory _itemCategory;

        private static Dictionary<string, InventoryItemSO> itemLookupTable;
        #endregion

        #region Properties
        public string ItemID => _itemID;
        public string DisplayName => _displayName;
        public string Description => _description;
        public Sprite Icon => _icon;
        public bool Stackable => _stackable;
        public float Price => _price;
        public ItemCategory ItemCategory => _itemCategory;
        #endregion

        /// <summary>
        /// Get the inventory _item from its ID
        /// </summary>
        /// <param name="itemID">
        /// String UUID that persists between game instances.
        /// </param>
        /// <returns></returns>
        public static InventoryItemSO GetItemFromID(string itemID)
        {
            // Make sure itemLookupTable has been instantiated before getting _item from it
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
                return null;

            return itemLookupTable[itemID];
        }

        /// <summary>
        /// Spawn pickup into the world
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Pickup SpawnPickup(Vector3 position, int number)
        {
            var pickup = Instantiate(this.pickup);
            pickup.transform.position = position;
            pickup.SetUp(this, number);

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