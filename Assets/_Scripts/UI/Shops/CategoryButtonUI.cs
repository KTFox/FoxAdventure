using UnityEngine;
using UnityEngine.UI;
using RPG.Inventories;
using RPG.Shops;
using System;

namespace RPG.UI.Shops
{
    public class CategoryButtonUI : MonoBehaviour
    {
        [SerializeField]
        private ItemCategory category;

        private Shop currentShop;
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void Start()
        {
            button.onClick.AddListener(SelectFilter);
        }

        void SelectFilter()
        {
            currentShop.SelectFilter(category);
        }

        public void RefreshUI()
        {
            button.interactable = currentShop.CurrentCategory != category;
        }

        public void SetShop(Shop shop)
        {
            currentShop = shop;
        }
    }
}
