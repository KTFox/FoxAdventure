using UnityEngine;
using RPG.Shops;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        private Shop currentShop;
        private Shopper shopper;

        private void Awake()
        {
            shopper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();

            shopper.OnActiveShopChanged += ShopChanged;
        }

        void ShopChanged()
        {
            currentShop = shopper.ActiveShop;
            gameObject.SetActive(currentShop != null);
        }

        private void Start()
        {
            ShopChanged();
        }
    }
}
