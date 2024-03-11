using UnityEngine.UI;
using UnityEngine;
using RPG.Inventories;
using RPG.Utility.UI;
using RPG.Abilities;

namespace RPG.UI.Inventories
{
    public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItemSO>
    {
        [SerializeField]
        private InventoryItemIcon icon;
        [SerializeField]
        private int index;
        [SerializeField]
        private Image cooldownVisual;

        private ActionStore store;
        private CooldownStore cooldownStore;

        #region Properties
        public InventoryItemSO Item => store.GetActionItem(index);
        public int ItemQuantity => store.GetItemQuantity(index);
        #endregion

        private void Awake()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            store = player.GetComponent<ActionStore>();
            cooldownStore = player.GetComponent<CooldownStore>();

            store.OnActionStoreUpdated += UpdateIcon;
        }

        private void Update()
        {
            cooldownVisual.fillAmount = cooldownStore.GetFractionTime(Item);
        }

        void UpdateIcon()
        {
            icon.SetItem(Item, ItemQuantity);
        }

        public void AddItems(InventoryItemSO item, int quantity)
        {
            store.AddActionItem(item, index, quantity);
        }

        public void RemoveItems(int quantity)
        {
            store.RemoveActionItem(index, quantity);
        }

        public int GetMaxAcceptable(InventoryItemSO item)
        {
            return store.GetMaxAcceptable(item, index);
        }
    }
}
