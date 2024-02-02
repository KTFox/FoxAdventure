using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI.Inventory {
    public class DragItem<T> : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler where T : class {

        public void OnBeginDrag(PointerEventData eventData) {
            Debug.Log("OnBeginDrag");
        }

        public void OnDrag(PointerEventData eventData) {
            Debug.Log("OnDrag");
        }

        public void OnEndDrag(PointerEventData eventData) {
            Debug.Log("OnEndDrag");
        }
    }
}
