using UnityEngine;
using RPG.Inventories;
using RPG.Utility.UI;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// An slot for the players equipment.
    /// </summary>
    public class EquipmentSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItemSO>
    {
        [SerializeField]
        private InventoryItemIcon icon;
        [SerializeField]
        private EquipLocation equipLocation;

        private Equipment playerEquipment;

        #region Properties
        public InventoryItemSO Item
        {
            get => playerEquipment.GetItemInSlot(equipLocation);
        }

        public int ItemQuantity
        {
            get
            {
                if (Item == null)
                    return 0;
                else
                    return 1;
            }
        }
        #endregion

        private void Awake()
        {
            playerEquipment = GameObject.FindGameObjectWithTag("Player").GetComponent<Equipment>();
            playerEquipment.OnEquipmentUpdated += RedrawUI;
        }

        private void Start()
        {
            RedrawUI();
        }

        private void RedrawUI()
        {
            icon.SetItem(playerEquipment.GetItemInSlot(equipLocation));
        }

        public void AddItems(InventoryItemSO item, int quantity)
        {
            playerEquipment.AddItem(equipLocation, (EquipableItemSO)item);
        }

        public void RemoveItems(int quantity)
        {
            playerEquipment.RemoveItem(equipLocation);
        }

        public int GetMaxAcceptable(InventoryItemSO item)
        {
            EquipableItemSO equipableItem = item as EquipableItemSO;

            if (equipableItem == null)
                return 0;
            if (equipableItem.AllowedEquipLocation != equipLocation)
                return 0;
            if (Item != null)
                return 0;

            return 1;
        }
    }
}
