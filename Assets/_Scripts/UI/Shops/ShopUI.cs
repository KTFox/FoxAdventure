using UnityEngine;
using TMPro;
using RPG.Shops;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI shopName;

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

            if (currentShop != null)
            {
                shopName.text = currentShop.ShopName;
            }

            gameObject.SetActive(currentShop != null);
        }

        private void Start()
        {
            ShopChanged();
        }

        public void CloseShopUI()
        {
            shopper.SetActiveShop(null);
        }
    }
}
