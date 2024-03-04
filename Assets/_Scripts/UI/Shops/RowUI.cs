using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RPG.Shops;

namespace RPG.UI.Shops
{
    public class RowUI : MonoBehaviour
    {
        [SerializeField]
        private Image iconImage;
        [SerializeField]
        private TextMeshProUGUI itemName;
        [SerializeField]
        private TextMeshProUGUI availabilityNumber;
        [SerializeField]
        private TextMeshProUGUI priceNumber;
        [SerializeField]
        private TextMeshProUGUI quantityNumber;

        public void Setup(ShopItem item)
        {
            iconImage.sprite = item.ItemIcon;
            itemName.text = item.ItemName;
            availabilityNumber.text = item.Availability.ToString();
            priceNumber.text = $"${item.Price:N2}";
            quantityNumber.text = $"- {item.QuantityInTransaction} +";
        }
    }
}
