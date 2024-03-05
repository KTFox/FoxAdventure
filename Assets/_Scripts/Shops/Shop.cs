using System;
using UnityEngine;
using System.Collections.Generic;
using RPG.Inventories;
using RPG.Control;
using RPG.Stats;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour, IRaycastable
    {
        public event Action OnShopUpdated;

        [SerializeField]
        private string _shopName;

        [Range(0, 100)]
        [SerializeField]
        private float sellingDiscountPercentage;

        [SerializeField]
        private StockItemConfig[] stockConfigs;

        [System.Serializable]
        private class StockItemConfig
        {
            public InventoryItemSO item;
            public int initialStock;

            [Range(0, 100)]
            public float buyingDiscountPercentage;

            public int levelToUnlock = 1;
        }

        private Shopper currentShopper;
        private Dictionary<InventoryItemSO, int> transactions = new Dictionary<InventoryItemSO, int>();
        private Dictionary<InventoryItemSO, int> stocksSold = new Dictionary<InventoryItemSO, int>();
        private bool _isBuyingMode = true;
        private ItemCategory _currentCategory;

        #region Properties
        public string ShopName
        {
            get => _shopName;
        }

        public bool IsBuyingMode
        {
            get => _isBuyingMode;
        }

        public ItemCategory CurrentCategory
        {
            get => _currentCategory;
        }
        #endregion

        public void SetShopper(Shopper shopper)
        {
            currentShopper = shopper;
        }

        public IEnumerable<ShopItem> GetFilteredItems()
        {
            foreach (ShopItem shopItem in GetAllItems())
            {
                if (_currentCategory == ItemCategory.None || shopItem.Item.ItemCategory == _currentCategory)
                {
                    yield return shopItem;
                }
            }
        }

        public IEnumerable<ShopItem> GetAllItems()
        {
            Dictionary<InventoryItemSO, int> availabilities = GetAvailabillities();
            Dictionary<InventoryItemSO, float> prices = GetPrices();

            foreach (InventoryItemSO item in availabilities.Keys)
            {
                if (availabilities[item] <= 0) continue;

                // Actual price after discounting
                float price = prices[item];

                // Get quantity in transastion
                int quantityInTransaction;
                transactions.TryGetValue(item, out quantityInTransaction);

                // Get current stock 
                int stock = availabilities[item];

                yield return new ShopItem(item, stock, price, quantityInTransaction);
            }
        }

        public void SelectMode(bool isBuying)
        {
            _isBuyingMode = isBuying;

            OnShopUpdated?.Invoke();
        }

        public void ConfirmTransaction()
        {
            // Get Shopper's Inventories
            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            if (shopperInventory == null) return;

            // Get Shopper's Purse
            Purse shopperPurse = currentShopper.GetComponent<Purse>();
            if (shopperPurse == null) return;

            // Confirm transaction in buying/selling mode
            foreach (ShopItem shopItem in GetAllItems())
            {
                InventoryItemSO inventoryItem = shopItem.Item;
                int quantity = shopItem.QuantityInTransaction;
                float price = shopItem.Price;
                for (int i = 0; i < quantity; i++)
                {
                    if (_isBuyingMode)
                    {
                        // Buying transaction
                        BuyItems(shopperInventory, shopperPurse, inventoryItem, price);
                    }
                    else
                    {
                        // Selling transaction
                        SellItems(shopperInventory, shopperPurse, inventoryItem, price);
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

            int stock = GetAvailabillities()[item];
            if (transactions[item] + quantity > stock)
            {
                transactions[item] = stock;
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
            if (!HasInventorySpace()) return false;

            return true;
        }

        public bool HasSufficientFund()
        {
            if (!_isBuyingMode) return true;

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

        public void SelectFilter(ItemCategory category)
        {
            _currentCategory = category;

            OnShopUpdated?.Invoke();
        }

        private Dictionary<InventoryItemSO, int> GetAvailabillities()
        {
            Dictionary<InventoryItemSO, int> availabilities = new Dictionary<InventoryItemSO, int>();
            foreach (StockItemConfig stockConfig in GetAvailableConfigs())
            {
                if (_isBuyingMode)
                {
                    // Availabilities in Buying Mode

                    if (!availabilities.ContainsKey(stockConfig.item))
                    {
                        int soldItemAmount = 0;
                        stocksSold.TryGetValue(stockConfig.item, out soldItemAmount);
                        availabilities[stockConfig.item] = -soldItemAmount;
                    }
                    availabilities[stockConfig.item] += stockConfig.initialStock;
                }
                else
                {
                    // Availabilities in Selling Mode

                    availabilities[stockConfig.item] = ItemCountInInventory(stockConfig.item);
                }
            }

            return availabilities;
        }

        private Dictionary<InventoryItemSO, float> GetPrices()
        {
            Dictionary<InventoryItemSO, float> prices = new Dictionary<InventoryItemSO, float>();
            foreach (StockItemConfig stockConfig in GetAvailableConfigs())
            {
                if (_isBuyingMode)
                {
                    // Prices in Buying Mode

                    if (!prices.ContainsKey(stockConfig.item))
                    {
                        prices[stockConfig.item] = stockConfig.item.Price;
                    }

                    prices[stockConfig.item] *= (1 - stockConfig.buyingDiscountPercentage / 100);
                }
                else
                {
                    // Prices in Selling Mode

                    prices[stockConfig.item] = stockConfig.item.Price * (1 - sellingDiscountPercentage / 100);
                }
            }

            return prices;
        }

        private IEnumerable<StockItemConfig> GetAvailableConfigs()
        {
            foreach (StockItemConfig stockConfig in stockConfigs)
            {
                if (stockConfig.levelToUnlock > GetShopperLevel()) continue;
                yield return stockConfig;
            }
        }

        private int ItemCountInInventory(InventoryItemSO item)
        {
            // Get Shopper's Inventories
            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            if (shopperInventory == null) return 0;

            // Get item count in shopper inventory
            int total = 0;
            for (int i = 0; i < shopperInventory.SlotSize; i++)
            {
                if (item == shopperInventory.GetItemInSlot(i))
                {
                    total += shopperInventory.GetItemQuantityInSlot(i);
                }
            }

            return total;
        }

        private void BuyItems(Inventory shopperInventory, Purse shopperPurse, InventoryItemSO inventoryItem, float price)
        {
            if (shopperPurse.CurrentBalance < price) return;

            bool successTransaction = shopperInventory.AddItemToFirstEmptySlot(inventoryItem, 1);

            if (successTransaction)
            {
                // Update Transaction
                AddTransaction(inventoryItem, -1);

                // Update stocks sold dictionary
                if (!stocksSold.ContainsKey(inventoryItem))
                {
                    stocksSold[inventoryItem] = 0;
                }
                stocksSold[inventoryItem]++;

                // Update Shopper's Purse
                shopperPurse.UpdateBalance(-price);
            }
        }

        private void SellItems(Inventory shopperInventory, Purse shopperPurse, InventoryItemSO inventoryItem, float price)
        {
            int slotIndex = FindFirstItemSlot(shopperInventory, inventoryItem);
            if (slotIndex == -1) return;

            // Update Transaction
            AddTransaction(inventoryItem, -1);

            // Update Stocks sold dictionary
            if (!stocksSold.ContainsKey(inventoryItem))
            {
                stocksSold[inventoryItem] = 0;
            }
            stocksSold[inventoryItem]--;

            // Remove sold item from shopper's inventory
            shopperInventory.RemoveFromSlot(slotIndex, 1);

            // Update Shopper's Purse
            shopperPurse.UpdateBalance(price);
        }

        private int FindFirstItemSlot(Inventory inventory, InventoryItemSO item)
        {
            for (int i = 0; i < inventory.SlotSize; i++)
            {
                if (item == inventory.GetItemInSlot(i))
                {
                    return i;
                }
            }

            return -1;
        }

        private bool HasInventorySpace()
        {
            if (!_isBuyingMode) return true;

            // Get current shopper's inventory
            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            if (shopperInventory == null) return false;

            // Build flat items list
            List<InventoryItemSO> flatItemList = new List<InventoryItemSO>();
            foreach (ShopItem shopItem in GetAllItems())
            {
                InventoryItemSO inventoryItem = shopItem.Item;
                int quantity = shopItem.QuantityInTransaction;
                for (int i = 0; i < quantity; i++)
                {
                    flatItemList.Add(inventoryItem);
                }
            }

            return shopperInventory.HasSpaceFor(flatItemList);
        }

        private bool IsEmptyTransaction()
        {
            return transactions.Count == 0;
        }

        private int GetShopperLevel()
        {
            BaseStats shopperStat = currentShopper.GetComponent<BaseStats>();
            if (shopperStat == null) return 0;

            return shopperStat.CurrentLevel;
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
