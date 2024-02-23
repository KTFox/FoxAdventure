using RPG.Inventory;
using UnityEngine;

namespace RPG.UI.Inventory
{
    public interface IItemHolder
    {
        InventoryItemSO Item { get; }
    }
}
