using UnityEngine;
using UnityEngine.UI;
using RPG.Inventories;
using RPG.Shops;

namespace RPG.UI.Shops
{
    public class CategoryButtonUI : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private ItemCategory _itemCategory;
        private Shop _currentShop;
        private Button _button;


        // Methods

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(SelectFilter);
        }

        void SelectFilter()
        {
            _currentShop.SelectFilter(_itemCategory);
        }

        public void RefreshUI()
        {
            _button.interactable = _currentShop.CurrentCategory != _itemCategory;
        }

        public void SetShop(Shop shop)
        {
            _currentShop = shop;
        }
    }
}
