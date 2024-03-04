using UnityEngine;
using RPG.Inventories;

namespace RPG.Shops
{
    public class ShopItem : MonoBehaviour
    {
        private InventoryItemSO _item;
        private int _stock;
        private float _price;
        private int _quantityInTransaction;

        #region Properties
        public InventoryItemSO Item
        {
            get => _item;
        }

        public Sprite ItemIcon
        {
            get => _item.Icon;
        }

        public string ItemName
        {
            get => _item.DisplayName;
        }

        public int Stock
        {
            get => _stock;
        }

        public float Price
        {
            get => _price;
        }

        public int QuantityInTransaction
        {
            get => _quantityInTransaction;
        }
        #endregion

        public ShopItem(InventoryItemSO item, int stock, float price, int quantityInTransaction)
        {
            _item = item;
            _stock = stock;
            _price = price;
            _quantityInTransaction = quantityInTransaction;
        }
    }
}
