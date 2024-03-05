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
        private Dictionary<InventoryItemSO, int> transactions = new Dictionary<InventoryItemSO, int>();
        private Dictionary<InventoryItemSO, int> stocks = new Dictionary<InventoryItemSO, int>();

        #region Properties
        public string ShopName
        {
            get => _shopName;
        }

        public bool IsBuyingMode
        {
            get => true;
        }
        #endregion

        private void Awake()
        {
            foreach (StockItemConfig stockConfig in stockConfigs)
            {
                stocks[stockConfig.item] = stockConfig.initialStock;
            }
        }

        public void SetShopper(Shopper shopper)
        {
            currentShopper = shopper;
        }

        public IEnumerable<ShopItem> GetFilteredItems()
        {
            return GetAllItems();
        }

        public IEnumerable<ShopItem> GetAllItems()
        {
            foreach (StockItemConfig stockConfig in stockConfigs)
            {
                // Actual price after discounting
                float price = stockConfig.item.Price * (1 - stockConfig.buyingPercentageDiscount / 100);

                // Get quantity in transastion
                int quantityInTransaction;
                transactions.TryGetValue(stockConfig.item, out quantityInTransaction);

                // Get current stock 
                int currentStock = stocks[stockConfig.item];

                yield return new ShopItem(stockConfig.item, currentStock, price, quantityInTransaction);
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

            // Get Shopper's Purse
            Purse shopperPurse = currentShopper.GetComponent<Purse>();
            if (shopperPurse == null) return;

            // Transfer to or from Shopper's Inventory
            foreach (ShopItem shopItem in GetAllItems())
            {
                InventoryItemSO inventoryItem = shopItem.Item;
                int quantity = shopItem.QuantityInTransaction;
                float price = shopItem.Price;
                for (int i = 0; i < quantity; i++)
                {
                    if (shopperPurse.CurrentBalance < price) break;

                    bool successTransaction = shopperInventory.AddItemToFirstEmptySlot(inventoryItem, 1);

                    if (successTransaction)
                    {
                        AddTransaction(inventoryItem, -1);
                        stocks[inventoryItem]--;
                        shopperPurse.UpdateBalance(-price);
                    }
                }
            }

            OnShopUpdated?.Invoke();
        }

        public void AddTransaction(InventoryItemSO item, int quantity)
        {
            if (!transactions.ContainsKey(item))
            {
                transactions[item] = 0;
            }

            if (transactions[item] + quantity > stocks[item])
            {
                transactions[item] = stocks[item];
            }
            else
            {
                transactions[item] += quantity;
            }

            if (transactions[item] <= 0)
            {
                transactions.Remove(item);
            }

            OnShopUpdated?.Invoke();
        }

        public bool CanTransact()
        {
            if (IsEmptyTransaction()) return false;
            if (!HasSufficientFund()) return false;

            return true;
        }

        public bool HasSufficientFund()
        {
            Purse currentShopperPurse = currentShopper.GetComponent<Purse>();
            if (currentShopperPurse == null) return false;

            return currentShopperPurse.CurrentBalance >= GetTransactionTotal();
        }

        public float GetTransactionTotal()
        {
            float total = 0;
            foreach (ShopItem item in GetAllItems())
            {
                total += item.QuantityInTransaction * item.Price;
            }

            return total;
        }

        private bool IsEmptyTransaction()
        {
            return transactions.Count == 0;
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
