using UnityEngine;
using RPG.Inventories;

namespace RPG.Shops
{
    public class ShopItem
    {
        private InventoryItemSO _item;
        private int _stock;
        private float _price;
        private int _quantityInTransaction;

        #region Properties
        public InventoryItemSO Item => _item;
        public Sprite ItemIcon => _item.Icon;
        public string ItemName => _item.DisplayName;
        public int Stock => _stock;
        public float Price => _price;
        public int QuantityInTransaction => _quantityInTransaction;
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
