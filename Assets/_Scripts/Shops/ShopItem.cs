using UnityEngine;
using RPG.Inventory;

namespace RPG.Shops
{
    public class ShopItem : MonoBehaviour
    {
        private InventoryItemSO item;
        private int _availability;
        private float _price;
        private int _quantityInTransaction;

        #region Properties
        public Sprite ItemIcon
        {
            get => item.Icon;
        }

        public string ItemName
        {
            get => item.DisplayName;
        }

        public int Availability
        {
            get => _availability;
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

        public ShopItem(InventoryItemSO item, int availability, float price, int quantityInTransaction)
        {
            this.item = item;
            _availability = availability;
            _price = price;
            _quantityInTransaction = quantityInTransaction;
        }
    }
}
