using UnityEngine;
using RPG.Saving;

namespace RPG.Inventory
{
    /// <summary>
    /// Spawns pickups that should exist on first load in a level. 
    /// This automatically spawns the correct prefab for a given inventory item.
    /// </summary>
    public class PickupSpawner : MonoBehaviour, ISaveable
    {
        [SerializeField]
        private InventoryItemSO item;

        #region Properties
        /// <summary>
        /// Returns the pickup spawned by this class if it exists.
        /// </summary>
        /// <returns>Returns null if the pickup has been collected.</returns>
        public Pickup Pickup
        {
            get => GetComponentInChildren<Pickup>();
        }

        public bool IsCollected
        {
            get => Pickup == null;
        }
        #endregion

        private void Awake()
        {
            SpawnPickup();
        }

        private void SpawnPickup()
        {
            var spawnedPickup = item.SpawnPickup(transform.position);
            spawnedPickup.transform.SetParent(transform);
        }

        private void DestroyPickup()
        {
            if (Pickup)
            {
                Destroy(Pickup.gameObject);
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
