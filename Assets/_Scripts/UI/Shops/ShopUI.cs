using UnityEngine;
using UnityEngine.UI;
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
        private ShopItemRowUI rowUIPrefab;
        [SerializeField]
        private TextMeshProUGUI totalText;
        [SerializeField]
        private Button switchModeButton;
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

            ChangeCurrentShop();

            shopper.OnActiveShopChanged += ChangeCurrentShop;
            switchModeButton.onClick.AddListener(SwitchMode);
            confirmButton.onClick.AddListener(ConfirmTransaction);
        }

        void ChangeCurrentShop()
        {
            if (currentShop != null)
            {
                currentShop.OnShopUpdated -= RefreshShopUI;
            }

            currentShop = shopper.ActiveShop;
            gameObject.SetActive(currentShop != null);

            foreach (CategoryButtonUI button in GetComponentsInChildren<CategoryButtonUI>())
            {
                button.SetShop(currentShop);
            }

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
                ShopItemRowUI itemRow = Instantiate(rowUIPrefab, rowUIListRoot);
                itemRow.Setup(currentShop, item);
            }

            totalText.color = currentShop.HasSufficientFund() ? originalTotalTextColor : Color.red;
            totalText.text = $"Total: ${currentShop.GetTransactionTotal():N2}";
            confirmButton.interactable = currentShop.CanTransact();

            // Update switch button and confirm button text
            TextMeshProUGUI switchButtonText = switchModeButton.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI confirmButtonText = confirmButton.GetComponentInChildren<TextMeshProUGUI>();
            if (currentShop.IsBuyingMode)
            {
                switchButtonText.text = "Switch to selling";
                confirmButtonText.text = "Buy";
            }
            else
            {
                switchButtonText.text = "Switch to buying";
                confirmButtonText.text = "Sell";
            }

            // Update category button's interactable state 
            foreach (CategoryButtonUI button in GetComponentsInChildren<CategoryButtonUI>())
            {
                button.RefreshUI();
            }
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

        public void SwitchMode()
        {
            currentShop.SelectMode(!currentShop.IsBuyingMode);
        }
        #endregion
    }
}
