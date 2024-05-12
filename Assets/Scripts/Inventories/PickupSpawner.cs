using UnityEngine;
using RPG.Saving;

namespace RPG.Inventories
{
    public class PickupSpawner : MonoBehaviour, ISaveable
    {
        // Variables

        [SerializeField]
        private InventoryItemSO _inventoryItem;
        [SerializeField]
        private int _quantity = 1;

        // Properties

        public Pickup PickupToSpawn => GetComponentInChildren<Pickup>();
        public bool IsCollected => PickupToSpawn == null;


        // Methods

        private void Awake()
        {
            SpawnPickup();
        }

        private void SpawnPickup()
        {
            var spawnedPickup = _inventoryItem.SpawnPickup(transform.position, _quantity);
            spawnedPickup.transform.SetParent(transform);
        }

        private void DestroyPickup()
        {
            if (PickupToSpawn)
            {
                Destroy(PickupToSpawn.gameObject);
            }
        }

        #region ISaveable implements
        object ISaveable.CaptureState()
        {
            return IsCollected;
        }

        void ISaveable.RestoreState(object state)
        {
            bool shouldBeCollected = (bool)state;

            if (shouldBeCollected && !IsCollected)
            {
                DestroyPickup();
            }

            if (!shouldBeCollected && IsCollected)
            {
                SpawnPickup();
            }
        }
        #endregion
    }
}
