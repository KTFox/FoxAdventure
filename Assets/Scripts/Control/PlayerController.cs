using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using UnityEngine.EventSystems;

namespace RPG.Control {
    public class PlayerController : MonoBehaviour {

        enum CursorType {
            None,
            UI,
            Movement,
            Combat
        }

        [System.Serializable]
        struct CursorMapping {
            public CursorType cursorType;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField]
        private CursorMapping[] cursorMappings;

        private Mover mover;
        private Fighter fighter;
        private Health health;

        private void Awake() {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        private void Update() {
            if (InteractWithUI()) return;

            if (health.IsDeath()) {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithUI() {
            SetCursor(CursorType.UI);
            return EventSystem.current.IsPointerOverGameObject();
        }

        private bool InteractWithComponent() {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits) {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();

                foreach (IRaycastable raycastable in raycastables) {
                    if (raycastable.HandleRaycast(this)) {
                        SetCursor(CursorType.Combat);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Return Physics.Raycast(GetMouseRay())
        /// </summary>
        /// <returns></returns>
        private bool InteractWithMovement() {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit) {
                if (Input.GetMouseButton(1)) {
                    mover.StartMoveAction(hit.point, 1f);
                }

                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private void SetCursor(CursorType type) {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type) {
            foreach (CursorMapping mapping in cursorMappings) {
                if (mapping.cursorType == type) {
                    return mapping;
                };
            }

            return cursorMappings[0];
        }

        private static Ray GetMouseRay() {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}