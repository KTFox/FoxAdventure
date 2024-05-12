using UnityEngine;
using RPG.Utility.UI;

namespace RPG.UI.Inventories
{
    [RequireComponent(typeof(IItemHolder))]
    public class ItemTooltipSpawner : TooltipSpawner
    {
        #region TooltipSpawner implements
        public override bool CanCreateTooltip()
        {
            var item = GetComponent<IItemHolder>();

            if (!item.InventoryItemSO)
            {
                return false;
            }

            return true;
        }

        public override void UpdateTooltip(GameObject tooltip)
        {
            var itemTooltip = tooltip.GetComponent<ItemTooltip>();

            if (!itemTooltip) return;

            var item = GetComponent<IItemHolder>().InventoryItemSO;

            itemTooltip.SetUp(item);
        }
        #endregion
    }
}
