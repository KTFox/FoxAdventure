using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.Saving;

namespace RPG.Inventories
{
    public class ItemDropper : MonoBehaviour, ISaveable
    {
        // Structs

        [System.Serializable]
        private struct DropRecord
        {
            public string itemID;
            public SerializableVector3 position;
            public int number;
            public int sceneBuildIndex;
        }

        // Variables

        private List<Pickup> _droppedItems = new List<Pickup>();
        private List<DropRecord> _otherSceneDroppedItems = new List<DropRecord>();


        // Methods

        #region DropItem overloads
        public void DropItem(InventoryItemSO item, int quantity)
        {
            SpawnPickup(item, GetDropLocation(), quantity);
        }

        public void DropItem(InventoryItemSO item)
        {
            SpawnPickup(item, GetDropLocation(), 1);
        }
        #endregion

        public void SpawnPickup(InventoryItemSO item, Vector3 spawnPosition, int quantity)
        {
            var pickup = item.SpawnPickup(spawnPosition, quantity);
            _droppedItems.Add(pickup);
        }

        protected virtual Vector3 GetDropLocation()
        {
            return transform.position;
        }

        #region ISaveable implements
        object ISaveable.CaptureState()
        {
            RemovePickedUpDrops();
            var droppedItems = new List<DropRecord>();
            int sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

            foreach (Pickup pickup in _droppedItems)
            {
                var dropItem = new DropRecord();

                dropItem.itemID = pickup.Item.ItemID;
                dropItem.position = new SerializableVector3(pickup.transform.position);
                dropItem.number = pickup.Quantity;
                dropItem.sceneBuildIndex = sceneBuildIndex;

                droppedItems.Add(dropItem);
            }

            droppedItems.AddRange(_otherSceneDroppedItems);

            return droppedItems;
        }

        void ISaveable.RestoreState(object state)
        {
            var dropRecords = (List<DropRecord>)state;
            int sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

            _otherSceneDroppedItems.Clear();

            foreach (DropRecord record in dropRecords)
            {
                if (record.sceneBuildIndex != sceneBuildIndex)
                {
                    _otherSceneDroppedItems.Add(record);
                    continue;
                }

                var item = InventoryItemSO.GetItemFromID(record.itemID);
                var spawnPosition = record.position.ToVector();
                var number = record.number;

                SpawnPickup(item, spawnPosition, number);
            }
        }
        #endregion

        private void RemovePickedUpDrops()
        {
            var newList = new List<Pickup>();

            foreach (Pickup item in _droppedItems)
            {
                if (item != null)
                {
                    newList.Add(item);
                }
            }

            _droppedItems = newList;
        }
    }
}
