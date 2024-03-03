using System;
using UnityEngine;
using System.Collections.Generic;
using RPG.Inventory;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour
    {
        public struct shopItem
        {
            private InventoryItemSO item;
            private int availability;
            private float price;
            private int quantityInTransaction;
        }

        public event Action OnShopUndated;

        #region Properties
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
    }
}
