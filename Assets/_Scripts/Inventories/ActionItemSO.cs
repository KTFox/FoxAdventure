using UnityEngine;

namespace RPG.Inventories
{
    /// <summary>
    /// An inventory _item that can be placed in the action bar and "Used".
    /// </summary>
    /// <remarks>
    /// This class should be used as a base. Subclasses must implement the `Use` method.
    /// </remarks>
    public class ActionItemSO : InventoryItemSO
    {
        [Tooltip("If true, this item can be used a limited times. Otherwise, this item can be used infinite times.")]
        [SerializeField] 
        private bool _consumable = false;

        public bool Consumable
        {
            get => _consumable;
        }

        /// <summary>
        /// Trigger the use of this _item. Override to provide functionality.
        /// </summary>
        /// <param name="user">The character that is using this action.</param>
        public virtual void Use(GameObject user)
        {
            Debug.Log($"Using action: {DisplayName}");
        }
    }
}
