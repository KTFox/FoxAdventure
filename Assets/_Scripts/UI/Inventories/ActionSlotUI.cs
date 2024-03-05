using UnityEngine;
using RPG.Inventories;
using RPG.Utility.UI;

namespace RPG.UI.Inventories
{
    public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItemSO>
    {
        [SerializeField]
        private InventoryItemIcon icon;
        [SerializeField]
        private int index;

        private ActionStore store;

        #region Properties
        public InventoryItemSO Item
        {
            get => store.GetActionItem(index);
        }

        public int ItemQuantity
        {
            get => store.GetItemQuantity(index);
        }
        #endregion

        private void Awake()
        {
            store = GameObject.FindGameObjectWithTag("Player").GetComponent<ActionStore>();
            store.OnActionStoreUpdate += UpdateIcon;
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
