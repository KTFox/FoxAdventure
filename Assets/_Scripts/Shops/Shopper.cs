using System;
using UnityEngine;

namespace RPG.Shops
{
    public class Shopper : MonoBehaviour
    {
        public event Action OnActiveShopChanged;

        private Shop activeShop;

        public Shop ActiveShop
        {
            get => activeShop;
        }

        public void SetActiveShop(Shop shop)
        {
            activeShop = shop;
            OnActiveShopChanged?.Invoke();
        }
    }
}
