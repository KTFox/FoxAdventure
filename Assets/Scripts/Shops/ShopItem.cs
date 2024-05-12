using UnityEngine;
using RPG.Inventories;

namespace RPG.Shops
{
    public class ShopItem
    {
        // Variables

        private InventoryItemSO _inventoryItem;
        private int _stock;
        private float _price;
        private int _quantityInTransaction;

        // Properties

        public InventoryItemSO InventoryItem => _inventoryItem;
        public Sprite ItemIcon => _inventoryItem.Icon;
        public string ItemName => _inventoryItem.DisplayName;
        public int Stock => _stock;
        public float Price => _price;
        public int QuantityInTransaction => _quantityInTransaction;

        // Constructors

        public ShopItem(InventoryItemSO item, int stock, float price, int quantityInTransaction)
        {
            _inventoryItem = item;
            _stock = stock;
            _price = price;
            _quantityInTransaction = quantityInTransaction;
        }
    }
}
