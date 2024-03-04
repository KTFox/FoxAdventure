using UnityEngine;

namespace RPG.Inventories
{
    /// <summary>
    /// An inventory _item that can be placed in the action bar and "Used".
    /// </summary>
    /// <remarks>
    /// This class should be used as a base. Subclasses must implement the `Use` method.
    /// </remarks>
    [CreateAssetMenu(menuName = "ScriptableObject/Item/ActionItemSO")]
    public class ActionItemSO : InventoryItemSO
    {
        [Tooltip("Does an instance of this _item get consumed every time it's used.")]
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
            Debug.Log($"Using action: {this}");
        }
    }
}
