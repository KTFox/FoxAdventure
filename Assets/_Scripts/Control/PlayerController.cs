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
        [SerializeField]
        private CursorMapping[] cursorMappings;

        [System.Serializable]
        private struct CursorMapping
        {
            public CursorType cursorType;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        private Mover mover;
        private Health health;
        private ActionStore actionStore;

        private float maxNavMeshProjectionDistance = 1f;
        private bool isDraggingUI;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
            actionStore = GetComponent<ActionStore>();
        }

        private void Update()
        {
            if (InteractWithUI()) return;

            if (health.IsDeath)
            {
                SetCursor(CursorType.None);
                return;
            }

            UseAbilities();

            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithUI()
        {
            if (Input.GetMouseButtonUp(0))
            {
                isDraggingUI = false;
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isDraggingUI = true;
                }

                SetCursor(CursorType.UI);
                return true;
            }

            if (isDraggingUI)
            {
                return true;
            }

            return false;
        }

        private void UseAbilities()
        {
            for (int i = 0; i < 4; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    actionStore.UseActionItem(i, gameObject);
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
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);

            if (hasHit)
            {
                if (!mover.CanMoveTo(target)) return false;

                if (Input.GetMouseButton(1))
                {
                    mover.StartMoveAction(target, 1f);
                }

                SetCursor(CursorType.Movement);
                return true;
            }

            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;

            return true;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.cursorType == type)
                {
                    return mapping;
                }
            }

            return cursorMappings[0];
        }

        public static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}