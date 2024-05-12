using UnityEngine;

namespace RPG.Inventories
{
    public class ActionItemSO : InventoryItemSO
    {
        // Variables

        [Tooltip("If true, this _inventoryItem can be used a limited times. Otherwise, this _inventoryItem can be used infinite times.")]
        [SerializeField] 
        private bool _consumable = false;

        // Properties

        public bool Consumable => _consumable;


        // Methods

        /// <summary>
        /// Trigger the use of this _inventoryItem. Override to provide functionality.
        /// </summary>
        /// <param name="user">The character that is using this action.</param>
        public virtual bool UseActionItem(GameObject user)
        {
            Debug.Log($"Using action: {DisplayName}");

            return false;
        }
    }
}
