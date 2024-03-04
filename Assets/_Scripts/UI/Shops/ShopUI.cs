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
        [SerializeField]
        private TextMeshProUGUI totalAmount;

        private Shop currentShop;
        private Shopper shopper;

        private void Awake()
        {
            shopper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();

            shopper.OnActiveShopChanged += ShopChanged;
        }

        void ShopChanged()
        {
            if (currentShop != null)
            {
                currentShop.OnShopUpdated -= RefreshShopUI;
            }

            currentShop = shopper.ActiveShop;
            gameObject.SetActive(currentShop != null);

            if (currentShop == null) return;

            shopName.text = currentShop.ShopName;
            currentShop.OnShopUpdated += RefreshShopUI;

            RefreshShopUI();
        }

        void RefreshShopUI()
        {
            foreach (Transform transform in rowUIListRoot)
            {
                Destroy(transform.gameObject);
            }

            foreach (ShopItem item in currentShop.GetFilteredItems())
            {
                RowUI itemRow = Instantiate(rowUIPrefab, rowUIListRoot);
                itemRow.Setup(currentShop, item);
            }

            totalAmount.text = $"Total: ${currentShop.GetTransactionTotal():N2}";
        }

        private void Start()
        {
            ShopChanged();
        }

        #region Unity events
        public void CloseShopUI()
        {
            shopper.SetActiveShop(null);
        }

        public void ConfirmTransaction()
        {
            currentShop.ConfirmTransaction();
        }
        #endregion
    }
}
