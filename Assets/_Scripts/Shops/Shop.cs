using System;
using UnityEngine;
using System.Collections.Generic;
using RPG.Inventory;
using RPG.Control;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour, IRaycastable
    {
        public struct shopItem
        {
            private InventoryItemSO item;
            private int availability;
            private float price;
            private int quantityInTransaction;
        }

        public event Action OnShopUndated;

        [SerializeField]
        private string _shopName;

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

        public IEnumerable<shopItem> GetFilteredItems()
        {
            return null;
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
