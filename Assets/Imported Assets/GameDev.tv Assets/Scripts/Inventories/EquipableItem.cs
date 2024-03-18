using UnityEngine;

namespace GameDevTV.Inventories
{
    /// <summary>
    /// An _inventory _inventoryItem that can be equipped to the _player. Weapons could be a
    /// subclass of this.
    /// </summary>
    [CreateAssetMenu(menuName = ("GameDevTV/GameDevTV.UI.Inventories/Equipable InventoryItem"))]
    public class EquipableItem : InventoryItem
    {
        // CONFIG DATA
        [Tooltip("Where are we allowed to put this _inventoryItem.")]
        [SerializeField] EquipLocation allowedEquipLocation = EquipLocation.Weapon;

        // PUBLIC

        public EquipLocation GetAllowedEquipLocation()
        {
            return allowedEquipLocation;
        }
    }
}