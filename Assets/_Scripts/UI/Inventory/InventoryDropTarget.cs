using UnityEngine;
using RPG.Inventory;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// Handles spawning pickups when item dropped into the world.
    /// Must be placed on the roof canvas where items can be dragged. 
    /// Will be called if dropped over empty space. 
    /// </summary>
    public class InventoryDropTarget : MonoBehaviour, IDragDestination<InventoryItemSO>
    {
        #region IDragDestination implements
        public void AddItems(InventoryItemSO item, int quantity)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<ItemDropper>().DropItem(item);
        }

        public int GetMaxAcceptable(InventoryItemSO item)
        {
            return int.MaxValue;
        }
        #endregion
    }
}
