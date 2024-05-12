using UnityEngine;
using RPG.Inventories;
using RPG.Utility.UI;

namespace RPG.UI.Inventories
{
    public class InventoryDropTarget : MonoBehaviour, IDragDestination<InventoryItemSO>
    {
        #region IDragDestination implements
        public void AddItems(InventoryItemSO inventoryItemSO, int quantity)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<ItemDropper>().DropItem(inventoryItemSO, quantity);
        }

        public int GetMaxAcceptable(InventoryItemSO inventoryItemSO)
        {
            return int.MaxValue;
        }
        #endregion
    }
}
