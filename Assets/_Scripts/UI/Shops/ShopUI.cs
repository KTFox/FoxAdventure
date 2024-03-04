using UnityEngine;
using TMPro;
using RPG.Shops;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI shopName;
        [SerializeField]
        private Transform rowUIListRoot;
        [SerializeField]
        private RowUI rowUIPrefab;

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

            if (currentShop != null)
            {
                shopName.text = currentShop.ShopName;
            }

            RefreshShopUI();
        }

        private void RefreshShopUI()
        {
            foreach (Transform transform in rowUIListRoot)
            {
                Destroy(transform.gameObject);
            }

            if (currentShop != null)
            {
                foreach (ShopItem item in currentShop.GetFilteredItems())
                {
                    Instantiate(rowUIPrefab, rowUIListRoot);
                }
            }
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
