using TMPro;
using UnityEngine;
using RPG.Inventories;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// Root of the tooltip prefab to expose properties to other class
    /// </summary>
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI title;
        [SerializeField]
        private TextMeshProUGUI description;

        public void SetUp(InventoryItemSO item)
        {
            title.text = item.DisplayName;
            description.text = item.Description;
        }
    }
}
