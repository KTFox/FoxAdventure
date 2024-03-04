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

        private Shop currentShop;
        private ShopItem shopItem;

        public void Setup(Shop currentShop, ShopItem shopItem)
        {
            iconImage.sprite = shopItem.ItemIcon;
            itemName.text = shopItem.ItemName;
            availabilityNumber.text = shopItem.Stock.ToString();
            priceNumber.text = $"${shopItem.Price:N2}";
            quantityNumber.text = $"{shopItem.QuantityInTransaction}";

            this.currentShop = currentShop;
            this.shopItem = shopItem;
        }

        #region Unity events
        public void AddQuantity()
        {
            currentShop.AddTransaction(shopItem.Item, 1);
        }

        public void RemoveQuantity()
        {
            currentShop.AddTransaction(shopItem.Item, -1);
        }
        #endregion
    }
}
