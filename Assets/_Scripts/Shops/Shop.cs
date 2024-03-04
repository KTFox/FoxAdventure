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
            yield return new ShopItem(InventoryItemSO.GetItemFromID("9e9be8c0-607a-4a6c-8c87-8d0d2aa53b5b"), 3, 12.32f, 10);
            yield return new ShopItem(InventoryItemSO.GetItemFromID("a8207449-cb19-4c59-9f2d-4d6bf339b1d5"), 2, 10f, 12);
            yield return new ShopItem(InventoryItemSO.GetItemFromID("c63a163a-0f2e-4e72-917c-b2ad73851bc2"), 1, 13f, 3);
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
