using RPG.Inventories;
using UnityEngine;

namespace RPG.UI.Inventories
{
    public class InventoryUI : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private InventorySlotUI _inventorySlotUIPrefab;
        private Inventory _playerInventory;


        // Methods

        private void Awake()
        {
            _playerInventory = Inventory.PlayerInventory;
        }

        private void OnEnable()
        {
            _playerInventory.OnInventoryUpdated += _playerInventory_InventoryUpdated;
        }

        private void Start()
        {
            _playerInventory_InventoryUpdated();
        }

        void _playerInventory_InventoryUpdated()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < _playerInventory.SlotSize; i++)
            {
                var inventorySlotUI = Instantiate(_inventorySlotUIPrefab, transform);
                inventorySlotUI.SetUp(_playerInventory, i);
            }
        }
    }
}
