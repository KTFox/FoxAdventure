using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    /// <summary>
    /// A ScriptableObject that represents any item that can be put in an inventory
    /// </summary>
    /// <remarks>
    /// In practice, you are likely to use a subclass such as "ActionItemSO" or "EquipableItemSO"
    /// </remarks>
    [CreateAssetMenu(menuName = "UI/InventorySystem/InventoryItemSO")]
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

        [Tooltip("The UI icon to represent this item in the inventory.")]
        [SerializeField]
        private Sprite _icon;

        [Tooltip("The prefab that should be spawned when this item is dropped.")]
        [SerializeField]
        private Pickup pickup;

        [Tooltip("If true, multiple items of this type can be stacked in the same inventory slot.")]
        [SerializeField]
        private bool _stackable;

        private static Dictionary<string, InventoryItemSO> itemLookupTable;
        #endregion

        #region Properties
        public string ItemID
        {
            get => _itemID;
        }

        public string DisplayName
        {
            get => _displayName;
        }

        public string Description
        {
            get => _description;
        }

        public Sprite Icon
        {
            get => _icon;
        }

        public bool Stackable
        {
            get => _stackable;
        }
        #endregion

        /// <summary>
        /// Get the inventory item from its ID
        /// </summary>
        /// <param name="itemID">
        /// String UUID that persists between game instances.
        /// </param>
        /// <returns></returns>
        public static InventoryItemSO GetItemFromID(string itemID)
        {
            // Make sure itemLookupTable has been instantiated before getting item from it
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