using System;
using UnityEngine;
using System.Collections.Generic;
using RPG.Inventory;
using RPG.Control;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour, IRaycastable
    {
        public event Action OnShopUpdated;

        [SerializeField]
        private string _shopName;

        [SerializeField]
        private StockItemConfig[] stockConfigs;

        [System.Serializable]
        private class StockItemConfig
        {
            public InventoryItemSO item;
            public int initialStock;

            [Range(0, 100)]
            public float buyingPercentageDiscount;
        }

        #region Properties
        public string ShopName
        {
            get => _shopName;
        }

        public bool IsBuyingMode
        {
            get => true;
        }

        public bool CanTransact
        {
            get => true;
        }
        #endregion

        public IEnumerable<ShopItem> GetFilteredItems()
        {
            foreach (StockItemConfig stockConfig in stockConfigs)
            {
                float price = stockConfig.item.Price * (1 - stockConfig.buyingPercentageDiscount / 100);
                yield return new ShopItem(stockConfig.item, stockConfig.initialStock, price, 0);
            }
        }

        public void SelectFilter(ItemCategory category)
        {

        }

        public ItemCategory GetCategory()
        {
            return ItemCategory.None;
        }

        public void SelectMode(bool isBuying)
        {

        }

        public void ConfirmTransaction()
        {

        }

        public void AddTransaction(InventoryItemSO item, int quantity)
        {

        }

        public float TransactionTotal()
        {
            return 0;
        }

        #region IRaycastable implements
        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<Shopper>().SetActiveShop(this);
            }

            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Shop;
        }
        #endregion
    }
}
