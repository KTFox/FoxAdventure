using System;
using UnityEngine;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// An inventory _item that can be placed in the action bar and "Used".
    /// </summary>
    /// <remarks>
    /// This class should be used as a base. Subclasses must implement the `Use`
    /// method.
    /// </remarks>
    [CreateAssetMenu(menuName = ("GameDevTV/GameDevTV.UI.Inventory/Action Item"))]
    public class ActionItem : InventoryItem
    {
        // CONFIG DATA
        [Tooltip("Does an instance of this _item get consumed every time it's used.")]
        [SerializeField] bool consumable = false;

        // PUBLIC

        /// <summary>
        /// Trigger the use of this _item. Override to provide functionality.
        /// </summary>
        /// <param name="user">The character that is using this action.</param>
        public virtual void Use(GameObject user)
        {
            Debug.Log("Using action: " + this);
        }

        public bool isConsumable()
        {
            return consumable;
        }
    }
}