using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Inventory
{
    /// <summary>
    /// To be placed on anything that wishes to drop pickups into the world.
    /// Tracks the drops for saving and restoring.
    /// </summary>
    public class ItemDropper : MonoBehaviour, ISaveable
    {
        private List<Pickup> droppedItems = new List<Pickup>();

        /// <summary>
        /// Create a pickup at the current position
        /// </summary>
        /// <param name="item"></param>
        public void DropItem(InventoryItemSO item)
        {
            SpawnPickup(item, GetDropLocation());
        }

        public void SpawnPickup(InventoryItemSO item, Vector3 spawnPosition)
        {
            var pickup = item.SpawnPickup(spawnPosition);
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
            DropRecord[] droppedItemArray = new DropRecord[droppedItems.Count];

            for (int i = 0; i < droppedItems.Count; i++)
            {
                droppedItemArray[i].itemID = droppedItems[i].Item.ItemID;
                droppedItemArray[i].position = new SerializableVector3(droppedItems[i].transform.position);
            }

            return droppedItemArray;
        }

        void ISaveable.RestoreState(object state)
        {
            DropRecord[] dropRecords = (DropRecord[])state;

            foreach (DropRecord record in dropRecords)
            {
                InventoryItemSO item = InventoryItemSO.GetItemFromID(record.itemID);
                Vector3 spawnPosition = record.position.ToVector();

                SpawnPickup(item, spawnPosition);
            }
        }
        #endregion

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

        #region Structs
        [System.Serializable]
        private struct DropRecord
        {
            public string itemID;
            public SerializableVector3 position;
        }
        #endregion
    }
}
