using UnityEngine;

namespace RPG.UI.Inventory {
    public class InventorySlotUI : MonoBehaviour, IDragContainer<Sprite> {

        public Sprite Item => throw new System.NotImplementedException();

        public int ItemQuanity => throw new System.NotImplementedException();

        public void AddItems(Sprite item, int quantity) {
            throw new System.NotImplementedException();
        }

        public int GetMaxAcceptable(Sprite item) {
            throw new System.NotImplementedException();
        }

        public void RemoveItems(int quantity) {
            throw new System.NotImplementedException();
        }
    }
}
