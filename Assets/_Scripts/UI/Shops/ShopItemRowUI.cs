using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RPG.Shops;

namespace RPG.UI.Shops
{
    public class ShopItemRowUI : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private Image _iconImage;
        [SerializeField]
        private TextMeshProUGUI _itemNameText;
        [SerializeField]
        private TextMeshProUGUI _stockValueText;
        [SerializeField]
        private TextMeshProUGUI _priceText;
        [SerializeField]
        private TextMeshProUGUI _quantityText;

        private Shop _currentShop;
        private ShopItem _shopItem;


        // Methods

        public void Setup(Shop shop, ShopItem shopItem)
        {
            _iconImage.sprite = shopItem.ItemIcon;
            _itemNameText.text = shopItem.ItemName;
            _stockValueText.text = shopItem.Stock.ToString();
            _priceText.text = $"${shopItem.Price:N2}";
            _quantityText.text = $"{shopItem.QuantityInTransaction}";
            _currentShop = shop;
            _shopItem = shopItem;
        }

        #region Unity events
        public void AddQuantity()
        {
            _currentShop.AddTransaction(_shopItem.InventoryItem, 1);
        }

        public void RemoveQuantity()
        {
            _currentShop.AddTransaction(_shopItem.InventoryItem, -1);
        }
        #endregion
    }
}
