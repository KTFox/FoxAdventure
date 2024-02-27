using UnityEngine;

namespace RPG.Inventory
{
    /// <summary>
    /// An inventory item that can be placed in the action bar and "Used".
    /// </summary>
    /// <remarks>
    /// This class should be used as a base. Subclasses must implement the `Use` method.
    /// </remarks>
    [CreateAssetMenu(menuName = "UI/InventorySystem/ActionItemSO")]
    public class ActionItemSO : InventoryItemSO
    {
        [Tooltip("Does an instance of this item get consumed every time it's used.")]
        [SerializeField] 
        private bool _consumable = false;

        public bool Consumable
        {
            get => _consumable;
        }

        /// <summary>
        /// Trigger the use of this item. Override to provide functionality.
        /// </summary>
        /// <param name="user">The character that is using this action.</param>
        public virtual void Use(GameObject user)
        {
            Debug.Log($"Using action: {this}");
        }
    }
}
