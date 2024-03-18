using System;
using UnityEngine;

namespace RPG.Shops
{
    public class Shopper : MonoBehaviour
    {

        private Shop _activeShop;

        public Shop ActiveShop => _activeShop;

        public event Action OnActiveShopChanged;

        public void SetActiveShop(Shop shop)
        {
            if (_activeShop != null)
            {
                _activeShop.CurrentShopper = null;
            }

            _activeShop = shop;

            if (_activeShop != null)
            {
                _activeShop.CurrentShopper = this;
            }

            OnActiveShopChanged?.Invoke();
        }
    }
}
