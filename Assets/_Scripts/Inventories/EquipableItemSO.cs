using UnityEngine;

namespace RPG.Inventories
{
    /// <summary>
    /// An inventory _item that can be equipped to the player. 
    /// Weapons could be a subclass of this.
    /// </summary>
    public class EquipableItemSO : InventoryItemSO
    {
        [Tooltip("Where are we allowed to put this _item.")]
        [SerializeField] EquipLocation _allowedEquipLocation;

        public EquipLocation AllowedEquipLocation
        {
            get => _allowedEquipLocation;
        }
    }
}
