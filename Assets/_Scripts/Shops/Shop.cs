using System;
using UnityEngine;
using System.Collections.Generic;
using RPG.Inventories;
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

        private Shopper currentShopper;
        private Dictionary<InventoryItemSO, int> transaction = new Dictionary<InventoryItemSO, int>();

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

        public void SetShopper(Shopper shopper)
        {
            currentShopper = shopper;
        }

        public IEnumerable<ShopItem> GetFilteredItems()
        {
            foreach (StockItemConfig stockConfig in stockConfigs)
            {
                float price = stockConfig.item.Price * (1 - stockConfig.buyingPercentageDiscount / 100);

                int quantityInTransaction;
                transaction.TryGetValue(stockConfig.item, out quantityInTransaction);

                yield return new ShopItem(stockConfig.item, stockConfig.initialStock, price, quantityInTransaction);
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
            // Get Shopper's Inventory
            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            if (shopperInventory == null) return;

            // Transfer to or from Shopper's Inventory
            var transactionSnapshot = new Dictionary<InventoryItemSO, int>(transaction);
            foreach (InventoryItemSO item in transactionSnapshot.Keys)
            {
                int quantity = transaction[item];
                for (int i = 0; i < quantity; i++)
                {
                    bool successTransaction = shopperInventory.AddItemToFirstEmptySlot(item, 1);

                    if (successTransaction)
                    {
                        AddTransaction(item, -1);
                    }
                }
            }
        }

        public void AddTransaction(InventoryItemSO item, int quantity)
        {
            if (!transaction.ContainsKey(item))
            {
                transaction[item] = 0;
            }

            transaction[item] += quantity;

            if (transaction[item] <= 0)
            {
                transaction.Remove(item);
            }

            OnShopUpdated?.Invoke();
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
