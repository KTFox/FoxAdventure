using UnityEngine;

namespace RPG.Inventory
{
    /// <summary>
    /// An inventory item that can be equipped to the player. 
    /// Weapons could be a subclass of this.
    /// </summary>
    [CreateAssetMenu(menuName = "UI/InventorySystem/EquipableItemSO")]
    public class EquipableItemSO : InventoryItemSO
    {
        [Tooltip("Where are we allowed to put this item.")]
        [SerializeField] EquipLocation _allowedEquipLocation;

        public EquipLocation AllowedEquipLocation
        {
            get => _allowedEquipLocation;
        }
    }
}
