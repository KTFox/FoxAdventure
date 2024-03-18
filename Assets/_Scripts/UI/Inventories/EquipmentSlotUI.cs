using UnityEngine;
using RPG.Inventories;
using RPG.Utility.UI;

namespace RPG.UI.Inventories
{
    public class EquipmentSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItemSO>
    {
        // Variables

        [SerializeField]
        private InventoryItemIcon _inventoryItemIcon;
        [SerializeField]
        private EquipLocation _equipLocation;
        private Equipment _playerEquipment;

        // Properties

        public InventoryItemSO InventoryItemSO => _playerEquipment.GetItemInSlot(_equipLocation);
        public int ItemQuantity => !InventoryItemSO ? 0 : 1;


        // Methods

        private void Awake()
        {
            _playerEquipment = GameObject.FindGameObjectWithTag("Player").GetComponent<Equipment>();

            _playerEquipment.OnEquipmentUpdated += _playerEquipment_EquipmentUpdated;
        }

        private void Start()
        {
            _playerEquipment_EquipmentUpdated();
        }

        void _playerEquipment_EquipmentUpdated()
        {
            _inventoryItemIcon.SetItem(_playerEquipment.GetItemInSlot(_equipLocation));
        }

        public void AddItems(InventoryItemSO inventoryItemSO, int quantity)
        {
            _playerEquipment.AddItem(_equipLocation, (EquipableItemSO)inventoryItemSO);
        }

        public void RemoveItems(int quantity)
        {
            _playerEquipment.RemoveItem(_equipLocation);
        }

        public int GetMaxAcceptable(InventoryItemSO inventoryItemSO)
        {
            var equipableItemSO = inventoryItemSO as EquipableItemSO;

            if (equipableItemSO == null)
            {
                return 0;
            }

            if (equipableItemSO.AllowedEquipLocation != _equipLocation)
            {
                return 0;
            }

            if (InventoryItemSO != null)
            {
                return 0;
            }

            return 1;
        }
    }
}
