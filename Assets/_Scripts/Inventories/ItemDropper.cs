using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.Saving;

namespace RPG.Inventories
{
    /// <summary>
    /// To be placed on anything that wishes to drop pickups into the world.
    /// Tracks the drops for saving and restoring.
    /// </summary>
    public class ItemDropper : MonoBehaviour, ISaveable
    {
        private List<Pickup> droppedItems = new List<Pickup>();
        private List<DropRecord> otherSceneDropItems = new List<DropRecord>();

        /// <summary>
        /// Create a pickup at GetDropLocation() position
        /// </summary>
        /// <param name="item"></param>
        /// <param name="number">
        /// The quantity of items contained in the pickup.
        /// Only used if the _item is stackable.
        /// </param>
        public void DropItem(InventoryItemSO item, int number)
        {
            SpawnPickup(item, GetDropLocation(), number);
        }

        /// <summary>
        /// Create a pickup at the current position
        /// </summary>
        /// <param name="item"></param>
        public void DropItem(InventoryItemSO item)
        {
            SpawnPickup(item, GetDropLocation(), 1);
        }

        public void SpawnPickup(InventoryItemSO item, Vector3 spawnPosition, int number)
        {
            var pickup = item.SpawnPickup(spawnPosition, number);
            droppedItems.Add(pickup);
        }

        /// <summary>
        /// Override to set a custom method for locating a drop
        /// </summary>
        /// <returns>The location that the drop should be spawned</returns>
        protected virtual Vector3 GetDropLocation()
        {
            return transform.position;
        }

        #region ISaveable implements
        object ISaveable.CaptureState()
        {
            RemovePickedupDrops();
            var droppedItemList = new List<DropRecord>();
            int sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
            foreach (Pickup pickup in droppedItems)
            {
                var dropItem = new DropRecord();
                dropItem.itemID = pickup.Item.ItemID;
                dropItem.position = new SerializableVector3(pickup.transform.position);
                dropItem.number = pickup.Number;
                dropItem.sceneBuildIndex = sceneBuildIndex;

                droppedItemList.Add(dropItem);
            }

            droppedItemList.AddRange(otherSceneDropItems);

            return droppedItemList;
        }

        void ISaveable.RestoreState(object state)
        {
            var dropRecords = (List<DropRecord>)state;
            int sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
            otherSceneDropItems.Clear();

            foreach (DropRecord record in dropRecords)
            {
                if (record.sceneBuildIndex != sceneBuildIndex)
                {
                    otherSceneDropItems.Add(record);
                    continue;
                }

                InventoryItemSO item = InventoryItemSO.GetItemFromID(record.itemID);
                Vector3 spawnPosition = record.position.ToVector();
                int number = record.number;

                SpawnPickup(item, spawnPosition, number);
            }
        }

        [System.Serializable]
        private struct DropRecord
        {
            public string itemID;
            public SerializableVector3 position;
            public int number;
            public int sceneBuildIndex;
        }

        /// <summary>
        /// Remove any drops in the world that have been picked up
        /// </summary>
        private void RemovePickedupDrops()
        {
            List<Pickup> newList = new List<Pickup>();

            foreach (Pickup item in droppedItems)
            {
                if (item != null)
                {
                    // Drop hasn't been picked up

                    newList.Add(item);
                }
            }

            droppedItems = newList;
        }
        #endregion
    }
}
