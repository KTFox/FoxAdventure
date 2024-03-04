using UnityEngine;
using RPG.Inventory;

namespace RPG.Shops
{
    public class ShopItem : MonoBehaviour
    {
        private InventoryItemSO item;
        private int availability;
        private float price;
        private int quantityInTransaction;

        public ShopItem(InventoryItemSO item, int availability, float price, int quantityInTransaction)
        {
            this.item = item;
            this.availability = availability;
            this.price = price;
            this.quantityInTransaction = quantityInTransaction;
        }
    }
}
