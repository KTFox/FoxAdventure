using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Utility.UI
{
    /// <summary>
    /// Abstract base class that handles the spawning of the tooltip prefab
    /// at the correct position on screen relative to the cursor.
    /// Override the TooltipSpawner to create a tooltip spawner for your own data.
    /// </summary>
    public abstract class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Tooltip("the prefab of the tooltip to spawn.")]
        [SerializeField]
        private GameObject tooltipPrefab;

        private GameObject tooltip;

        #region Abstract methods
        /// <summary>
        /// Called when it is time to update the information on the tooltip prefab.
        /// </summary>
        /// <param name="tooltip">
        /// The spawned tooltip prefab for updating.
        /// </param>
        public abstract void UpdateTooltip(GameObject tooltip);

        /// <summary>
        /// Return true when the tooltip spawner should be allowed to create a tooltip.
        /// </summary>
        public abstract bool CanCreateTooltip();
        #endregion

        private void OnDisable()
        {
            ClearTooltip();
        }

        private void OnDestroy()
        {
            ClearTooltip();
        }

        #region IPointerHandler implements
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            var parentCanvas = GetComponentInParent<Canvas>();

            if (tooltip && CanCreateTooltip())
            {
                ClearTooltip();
            }

            if (!tooltip && CanCreateTooltip())
            {
                tooltip = Instantiate(tooltipPrefab, parentCanvas.transform);
            }

            if (tooltip)
            {
                UpdateTooltip(tooltip);
                LocateTooltip();
            }
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            ClearTooltip();
        }
        #endregion

        private void LocateTooltip()
        {
            // Required to ensure corners are updated by positioning elements.
            Canvas.ForceUpdateCanvases();

            // Get tooltip corners
            var tooltipCorners = new Vector3[4];
            tooltip.GetComponent<RectTransform>().GetWorldCorners(tooltipCorners);

            // Get inventory slot corners
            var slotCorners = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(slotCorners);

            // Get relative position of the inventory slot
            bool below = transform.position.y < (Screen.height / 2);
            bool right = transform.position.x > (Screen.width / 2);

            // Get corner index
            int slotCornerIndex = GetCornerIndex(!below, !right);
            int tooltipCornerIndex = GetCornerIndex(below, right);

            // Set tooltip position
            Vector3 moveVector = slotCorners[slotCornerIndex] - tooltipCorners[tooltipCornerIndex];
            tooltip.transform.position += moveVector;
        }

        private int GetCornerIndex(bool below, bool right)
        {
            if (below && !right) return 0;
            else if (!below && !right) return 1;
            else if (!below && right) return 2;
            else return 3;
        }

        private void ClearTooltip()
        {
            if (tooltip)
            {
                Destroy(tooltip);
            }
        }
    }
}
