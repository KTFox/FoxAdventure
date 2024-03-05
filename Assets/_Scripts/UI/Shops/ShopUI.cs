using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPG.Shops;
using Unity.VisualScripting;

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
        private TextMeshProUGUI totalText;
        [SerializeField]
        private Button confirmButton;

        private Shop currentShop;
        private Shopper shopper;
        private Color originalTotalTextColor;

        private void Awake()
        {
            shopper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();
        }

        private void Start()
        {
            originalTotalTextColor = totalText.color;

            ShopChanged();

            shopper.OnActiveShopChanged += ShopChanged;
            confirmButton.onClick.AddListener(ConfirmTransaction);
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

            totalText.color = currentShop.HasSufficientFund() ? originalTotalTextColor : Color.red;
            totalText.text = $"Total: ${currentShop.GetTransactionTotal():N2}";
            confirmButton.interactable = currentShop.CanTransact();
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
