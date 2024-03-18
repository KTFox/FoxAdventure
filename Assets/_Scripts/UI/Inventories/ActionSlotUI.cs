using UnityEngine.UI;
using UnityEngine;
using RPG.Inventories;
using RPG.Utility.UI;
using RPG.Abilities;

namespace RPG.UI.Inventories
{
    public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItemSO>
    {
        // Variables

        [SerializeField]
        private InventoryItemIcon _inventoryItemIcon;
        [SerializeField]
        private int _slotIndex;
        [SerializeField]
        private Image cooldownVisual;

        private ActionStore _actionStore;
        private CooldownStore _cooldownStore;

        // Properties

        public InventoryItemSO InventoryItemSO => _actionStore.GetActionItem(_slotIndex);
        public int ItemQuantity => _actionStore.GetItemQuantity(_slotIndex);
        

        // Methods

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _actionStore = player.GetComponent<ActionStore>();
            _cooldownStore = player.GetComponent<CooldownStore>();

            _actionStore.OnActionStoreUpdated += _actionStore_ActionStoreUpdated;
        }

        void _actionStore_ActionStoreUpdated()
        {
            _inventoryItemIcon.SetItem(InventoryItemSO, ItemQuantity);
        }

        private void Update()
        {
            cooldownVisual.fillAmount = _cooldownStore.GetFractionTime(InventoryItemSO);
        }

        public void AddItems(InventoryItemSO inventoryItemSO, int quantity)
        {
            _actionStore.AddActionItem(inventoryItemSO, _slotIndex, quantity);
        }

        public void RemoveItems(int quantity)
        {
            _actionStore.RemoveActionItem(_slotIndex, quantity);
        }

        public int GetMaxAcceptable(InventoryItemSO inventoryItemSO)
        {
            return _actionStore.GetMaxAcceptable(inventoryItemSO, _slotIndex);
        }
    }
}
