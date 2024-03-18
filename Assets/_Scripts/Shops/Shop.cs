using System;
using UnityEngine;
using System.Collections.Generic;
using RPG.Inventories;
using RPG.Control;
using RPG.Stats;
using RPG.Saving;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour, IRaycastable, ISaveable
    {
        // Structs

        [System.Serializable]
        private class StockItemConfig
        {
            public InventoryItemSO item;
            public int initialStock;

            [Range(0, 100)]
            public float buyingDiscountPercentage;

            public int levelToUnlock = 1;
        }

        // Variables

        [SerializeField]
        private string _shopName;
        [Range(0, 100)]
        [SerializeField]
        private float _sellingDiscountPercentage;
        [SerializeField]
        private StockItemConfig[] _stockItemConfigs;
        [SerializeField]
        private float _maximumBarterDiscount;

        private Shopper _currentShopper;
        private Dictionary<InventoryItemSO, int> _transactions = new Dictionary<InventoryItemSO, int>();
        private Dictionary<InventoryItemSO, int> _soldItems = new Dictionary<InventoryItemSO, int>();
        private bool _isBuyingMode = true;
        private ItemCategory _currentCategory;

        // Properties

        public string ShopName => _shopName;
        public Shopper CurrentShopper
        {
            get => _currentShopper;
            set => _currentShopper = value;
        }
        public bool IsBuyingMode => _isBuyingMode;
        public ItemCategory CurrentCategory => _currentCategory;

        // Events

        public event Action OnShopUpdated;


        // Methods

        public IEnumerable<ShopItem> GetFilteredShopItems()
        {
            foreach (ShopItem shopItem in GetAllShopItems())
            {
                if (_currentCategory == ItemCategory.None || shopItem.InventoryItem.ItemCategory == _currentCategory)
                {
                    yield return shopItem;
                }
            }
        }

        public IEnumerable<ShopItem> GetAllShopItems()
        {
            Dictionary<InventoryItemSO, int> availabilities = GetAvailabillities();
            Dictionary<InventoryItemSO, float> prices = GetPrices();

            foreach (InventoryItemSO item in availabilities.Keys)
            {
                if (availabilities[item] <= 0) continue;

                float price = prices[item];
                int quantityInTransaction;
                int stock = availabilities[item];

                _transactions.TryGetValue(item, out quantityInTransaction);

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
            var shopperInventory = _currentShopper.GetComponent<Inventory>();
            if (shopperInventory == null) return;

            var shopperPurse = _currentShopper.GetComponent<Purse>();
            if (shopperPurse == null) return;

            foreach (ShopItem shopItem in GetAllShopItems())
            {
                InventoryItemSO inventoryItem = shopItem.InventoryItem;
                int quantity = shopItem.QuantityInTransaction;
                float price = shopItem.Price;

                for (int i = 0; i < quantity; i++)
                {
                    if (_isBuyingMode)
                    {
                        BuyItems(shopperInventory, shopperPurse, inventoryItem, price);
                    }
                    else
                    {
                        SellItems(shopperInventory, shopperPurse, inventoryItem, price);
                    }
                }
            }

            OnShopUpdated?.Invoke();
        }

        public void AddTransaction(InventoryItemSO item, int quantity)
        {
            if (!_transactions.ContainsKey(item))
            {
                _transactions[item] = 0;
            }

            int stock = GetAvailabillities()[item];

            if (_transactions[item] + quantity > stock)
            {
                _transactions[item] = stock;
            }
            else
            {
                _transactions[item] += quantity;
            }

            if (_transactions[item] <= 0)
            {
                _transactions.Remove(item);
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
            if (!_isBuyingMode)
            {
                return true;
            }

            var currentShopperPurse = _currentShopper.GetComponent<Purse>();

            if (currentShopperPurse == null)
            {
                return false;
            }

            return currentShopperPurse.CurrentBalance >= GetTransactionTotal();
        }

        public float GetTransactionTotal()
        {
            float total = 0;

            foreach (ShopItem item in GetAllShopItems())
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
            var availabilities = new Dictionary<InventoryItemSO, int>();

            foreach (StockItemConfig stockConfig in GetAvailableStockItemConfigs())
            {
                if (_isBuyingMode)
                {
                    if (!availabilities.ContainsKey(stockConfig.item))
                    {
                        int soldItemAmount = 0;
                        _soldItems.TryGetValue(stockConfig.item, out soldItemAmount);
                        availabilities[stockConfig.item] = -soldItemAmount;
                    }

                    availabilities[stockConfig.item] += stockConfig.initialStock;
                }
                else
                {
                    availabilities[stockConfig.item] = ItemCountInInventory(stockConfig.item);
                }
            }

            return availabilities;
        }

        private Dictionary<InventoryItemSO, float> GetPrices()
        {
            var prices = new Dictionary<InventoryItemSO, float>();

            foreach (StockItemConfig stockConfig in GetAvailableStockItemConfigs())
            {
                if (_isBuyingMode)
                {
                    if (!prices.ContainsKey(stockConfig.item))
                    {
                        prices[stockConfig.item] = stockConfig.item.Price * GetBarterDiscount();
                    }

                    prices[stockConfig.item] *= (1 - stockConfig.buyingDiscountPercentage / 100);
                }
                else
                {
                    prices[stockConfig.item] = stockConfig.item.Price * (1 - _sellingDiscountPercentage / 100);
                }
            }

            return prices;
        }

        private IEnumerable<StockItemConfig> GetAvailableStockItemConfigs()
        {
            foreach (StockItemConfig stockConfig in _stockItemConfigs)
            {
                if (stockConfig.levelToUnlock > GetShopperLevel()) continue;

                yield return stockConfig;
            }
        }

        private int ItemCountInInventory(InventoryItemSO item)
        {
            var shopperInventory = _currentShopper.GetComponent<Inventory>();

            if (shopperInventory == null)
            {
                return 0;
            }

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
                AddTransaction(inventoryItem, -1);

                if (!_soldItems.ContainsKey(inventoryItem))
                {
                    _soldItems[inventoryItem] = 0;
                }

                _soldItems[inventoryItem]++;
                shopperPurse.UpdateBalance(-price);
            }
        }

        private void SellItems(Inventory shopperInventory, Purse shopperPurse, InventoryItemSO inventoryItem, float price)
        {
            int slotIndex = FindFirstItemSlot(shopperInventory, inventoryItem);

            if (slotIndex == -1) return;

            AddTransaction(inventoryItem, -1);

            if (!_soldItems.ContainsKey(inventoryItem))
            {
                _soldItems[inventoryItem] = 0;
            }

            _soldItems[inventoryItem]--;
            shopperInventory.RemoveFromSlot(slotIndex, 1);
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
            if (!_isBuyingMode)
            {
                return true;
            }

            var shopperInventory = _currentShopper.GetComponent<Inventory>();

            if (shopperInventory == null)
            {
                return false;
            }

            var flatItemList = new List<InventoryItemSO>();

            foreach (ShopItem shopItem in GetAllShopItems())
            {
                InventoryItemSO inventoryItem = shopItem.InventoryItem;
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
            return _transactions.Count == 0;
        }

        private int GetShopperLevel()
        {
            var currentShopperBaseStats = _currentShopper.GetComponent<BaseStats>();

            if (currentShopperBaseStats == null)
            {
                return 0;
            }

            return currentShopperBaseStats.CurrentLevel;
        }

        private float GetBarterDiscount()
        {
            var currentShopperBaseStats = _currentShopper.GetComponent<BaseStats>();
            float discount = currentShopperBaseStats.GetValueOfStat(Stat.BuyingDiscountPercentage);

            return (1 - Mathf.Min(discount, _maximumBarterDiscount) / 100);
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

        #region ISaveable implements
        object ISaveable.CaptureState()
        {
            Dictionary<string, int> saveObject = new Dictionary<string, int>();
            foreach (var pair in _soldItems)
            {
                saveObject[pair.Key.ItemID] = pair.Value;
            }

            return saveObject;
        }

        void ISaveable.RestoreState(object state)
        {
            Dictionary<string, int> saveObject = (Dictionary<string, int>)state;
            _soldItems.Clear();
            foreach (var pair in saveObject)
            {
                _soldItems[InventoryItemSO.GetItemFromID(pair.Key)] = pair.Value;
            }
        }
        #endregion
    }
}
