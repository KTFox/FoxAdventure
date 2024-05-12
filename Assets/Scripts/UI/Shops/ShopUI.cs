using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPG.Shops;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private TextMeshProUGUI _shopNameText;
        [SerializeField]
        private Transform _rowUIRoot;
        [SerializeField]
        private ShopItemRowUI _rowUIPrefab;
        [SerializeField]
        private TextMeshProUGUI _totalText;
        [SerializeField]
        private Button _switchModeButton;
        [SerializeField]
        private Button _confirmButton;

        private Shop _currentShop;
        private Shopper _shopper;
        private Color _originalTotalTextColor;


        // Methods

        private void Awake()
        {
            _shopper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();
        }

        private void Start()
        {
            _originalTotalTextColor = _totalText.color;

            shopper_OnActiveShopChanged();

            _shopper.OnActiveShopChanged += shopper_OnActiveShopChanged;
            _switchModeButton.onClick.AddListener(SwitchMode);
            _confirmButton.onClick.AddListener(ConfirmTransaction);
        }

        void shopper_OnActiveShopChanged()
        {
            if (_currentShop != null)
            {
                _currentShop.OnShopUpdated -= RefreshShopUI;
            }

            _currentShop = _shopper.ActiveShop;
            gameObject.SetActive(_currentShop != null);

            foreach (var button in GetComponentsInChildren<CategoryButtonUI>())
            {
                button.SetShop(_currentShop);
            }

            if (_currentShop == null) return;

            _shopNameText.text = _currentShop.ShopName;
            _currentShop.OnShopUpdated += RefreshShopUI;
            RefreshShopUI();
        }

        void RefreshShopUI()
        {
            foreach (Transform transform in _rowUIRoot)
            {
                Destroy(transform.gameObject);
            }

            foreach (ShopItem shopItem in _currentShop.GetFilteredShopItems())
            {
                ShopItemRowUI shopItemRow = Instantiate(_rowUIPrefab, _rowUIRoot);
                shopItemRow.Setup(_currentShop, shopItem);
            }

            _totalText.color = _currentShop.HasSufficientFund() ? _originalTotalTextColor : Color.red;
            _totalText.text = $"Total: ${_currentShop.GetTransactionTotal():N2}";
            _confirmButton.interactable = _currentShop.CanTransact();

            var switchButtonText = _switchModeButton.GetComponentInChildren<TextMeshProUGUI>();
            var confirmButtonText = _confirmButton.GetComponentInChildren<TextMeshProUGUI>();

            if (_currentShop.IsBuyingMode)
            {
                switchButtonText.text = "Switch to selling";
                confirmButtonText.text = "Buy";
            }
            else
            {
                switchButtonText.text = "Switch to buying";
                confirmButtonText.text = "Sell";
            }

            foreach (var button in GetComponentsInChildren<CategoryButtonUI>())
            {
                button.RefreshUI();
            }
        }

        #region Unity events
        public void CloseShopUI()
        {
            _shopper.SetActiveShop(null);
        }

        public void ConfirmTransaction()
        {
            _currentShop.ConfirmTransaction();
        }

        public void SwitchMode()
        {
            _currentShop.SelectMode(!_currentShop.IsBuyingMode);
        }
        #endregion
    }
}
