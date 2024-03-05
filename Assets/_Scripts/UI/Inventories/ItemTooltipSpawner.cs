using UnityEngine;
using RPG.Utility.UI;

namespace RPG.UI.Inventories
{
    /// <summary>
    /// To be placed on the InventorySlot to spawn and show the correct _item tooltip.
    /// </summary>
    [RequireComponent(typeof(IItemHolder))]
    public class ItemTooltipSpawner : TooltipSpawner
    {
        #region TooltipSpawner implements
        public override bool CanCreateTooltip()
        {
            var item = GetComponent<IItemHolder>();

            if (!item.Item) return false;

            return true;
        }

        public override void UpdateTooltip(GameObject tooltip)
        {
            var itemTooltip = tooltip.GetComponent<ItemTooltip>();

            if (!itemTooltip) return;

            var item = GetComponent<IItemHolder>().Item;

            itemTooltip.SetUp(item);
        }
        #endregion
    }
}
