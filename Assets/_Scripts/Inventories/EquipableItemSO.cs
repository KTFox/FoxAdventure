using UnityEngine;

namespace RPG.Inventories
{
    public class EquipableItemSO : InventoryItemSO
    {
        [Tooltip("Where are we allowed to put this _inventoryItem.")]
        [SerializeField] EquipLocation _allowedEquipLocation;

        public EquipLocation AllowedEquipLocation => _allowedEquipLocation;
    }
}
