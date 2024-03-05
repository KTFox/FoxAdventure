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
        }

        private Shopper currentShopper;
        private Dictionary<InventoryItemSO, int> transactions = new Dictionary<InventoryItemSO, int>();
        private Dictionary<InventoryItemSO, int> stocks = new Dictionary<InventoryItemSO, int>();
        private bool _isBuyingMode = true;

        #region Properties
        public string ShopName
        {
            get => _shopName;
        }

        public bool IsBuyingMode
        {
            get => _isBuyingMode;
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
                float price = GetActualPriceInTransaction(stockConfig);

                // Get quantity in transastion
                int quantityInTransaction;
                transactions.TryGetValue(stockConfig.item, out quantityInTransaction);

                // Get current stock 
                int stock = GetActualStock(stockConfig.item);

                yield return new ShopItem(stockConfig.item, stock, price, quantityInTransaction);
            }
        }

        private float GetActualPriceInTransaction(StockItemConfig stockConfig)
        {
            float buyingPrice = stockConfig.item.Price * (1 - stockConfig.buyingDiscountPercentage / 100);

            if (_isBuyingMode)
            {
                return buyingPrice;
            }
            else
            {
                return buyingPrice * (1 - sellingDiscountPercentage / 100);
            }
        }

        private int GetActualStock(InventoryItemSO inventoryItem)
        {
            if (_isBuyingMode)
            {
                return stocks[inventoryItem];
            }
            else
            {
                return ItemCountInInventory(inventoryItem);
            }
        }

        private int ItemCountInInventory(InventoryItemSO item)
        {
            // Get Shopper's Inventory
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

        public void SelectMode(bool isBuying)
        {
            _isBuyingMode = isBuying;

            OnShopUpdated?.Invoke();
        }

        public void ConfirmTransaction()
        {
            // Get Shopper's Inventory
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

        private void BuyItems(Inventory shopperInventory, Purse shopperPurse, InventoryItemSO inventoryItem, float price)
        {
            if (shopperPurse.CurrentBalance < price) return;

            bool successTransaction = shopperInventory.AddItemToFirstEmptySlot(inventoryItem, 1);

            if (successTransaction)
            {
                AddTransaction(inventoryItem, -1);
                stocks[inventoryItem]--;
                shopperPurse.UpdateBalance(-price);
            }
        }

        private void SellItems(Inventory shopperInventory, Purse shopperPurse, InventoryItemSO inventoryItem, float price)
        {
            int slotIndex = FindFirstItemSlot(shopperInventory, inventoryItem);
            if (slotIndex == -1) return;

            AddTransaction(inventoryItem, -1);
            shopperInventory.RemoveFromSlot(slotIndex, 1);
            stocks[inventoryItem]++;
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

        public void AddTransaction(InventoryItemSO item, int quantity)
        {
            if (!transactions.ContainsKey(item))
            {
                transactions[item] = 0;
            }

            int stock = GetActualStock(item);
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

        public void SelectFilter(ItemCategory category)
        {

        }

        public ItemCategory GetCategory()
        {
            return ItemCategory.None;
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
