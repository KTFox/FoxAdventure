using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.AI;
using RPG.Movement;
using RPG.Attributes;
using RPG.Inventories;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        // Structs

        [Serializable]
        private struct CursorMapping
        {
            public CursorType cursorType;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        // Variables

        [SerializeField]
        private CursorMapping[] _cursorMappings;

        private Mover _mover;
        private Health _health;
        private ActionStore _actionStore;
        private bool _isDraggingUI;


        // Methods

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _health = GetComponent<Health>();
            _actionStore = GetComponent<ActionStore>();
        }

        private void Update()
        {
            if (InteractWithUI()) return;

            if (_health.IsDead)
            {
                SetCursor(CursorType.None);
                return;
            }

            UseAbilitiesFromActionStore();

            if (InteractWithComponent()) return;

            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        public static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private bool InteractWithUI()
        {
            if (Input.GetMouseButtonUp(0))
            {
                _isDraggingUI = false;
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _isDraggingUI = true;
                }

                SetCursor(CursorType.UI);

                return true;
            }

            if (_isDraggingUI)
            {
                return true;
            }

            return false;
        }

        private void UseAbilitiesFromActionStore()
        {
            for (int i = 0; i < 4; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    _actionStore.UseActionItem(i, gameObject);
                }
            }
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = GetAllSortedRaycastHit();

            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();

                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());

                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Get all raycast hits sorted by distance
        /// </summary>
        /// <returns></returns>
        private RaycastHit[] GetAllSortedRaycastHit()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);

            return hits;
        }

        private bool InteractWithMovement()
        {
            Vector3 targetPosition;
            bool hasHit = RaycastNavMesh(out targetPosition);

            if (hasHit)
            {
                if (!_mover.CanMoveTo(targetPosition))
                {
                    return false;
                }

                if (Input.GetMouseButton(1))
                {
                    _mover.StartMoveAction(targetPosition, 1f);
                }

                SetCursor(CursorType.Movement);

                return true;
            }

            return false;
        }

        private bool RaycastNavMesh(out Vector3 targetPosition)
        {
            targetPosition = new Vector3();

            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (!hasHit)
            {
                return false;
            }

            NavMeshHit navMeshHit;
            float maxNavMeshProjectionDistance = 1f;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);

            if (!hasCastToNavMesh)
            {
                return false;
            }

            targetPosition = navMeshHit.position;

            return true;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in _cursorMappings)
            {
                if (mapping.cursorType == type)
                {
                    return mapping;
                }
            }

            return _cursorMappings[0];
        }
    }
}