using TMPro;
using UnityEngine;
using RPG.Inventories;

namespace RPG.UI.Inventories
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _title;
        [SerializeField]
        private TextMeshProUGUI _description;

        public void SetUp(InventoryItemSO inventoryItemSO)
        {
            _title.text = inventoryItemSO.DisplayName;
            _description.text = inventoryItemSO.Description;
        }
    }
}
