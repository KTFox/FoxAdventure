using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Utility.UI
{
    public abstract class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        // Variables

        [Tooltip("the prefab of the _tooltip to spawn.")]
        [SerializeField]
        private GameObject _tooltipPrefab;

        private GameObject _tooltip;


        // Methods

        private void OnDisable()
        {
            ClearTooltip();
        }

        private void OnDestroy()
        {
            ClearTooltip();
        }

        #region Abstract methods
        /// <summary>
        /// Called when it is time to update the information on the tooltip prefab.
        /// </summary>
        /// <param name="tooltip">
        /// The spawned _tooltip prefab for updating.
        /// </param>
        public abstract void UpdateTooltip(GameObject tooltip);

        /// <summary>
        /// Return true when the _tooltip spawner should be allowed to create a tooltip.
        /// </summary>
        public abstract bool CanCreateTooltip();
        #endregion

        #region IPointerHandler implements
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            var parentCanvas = GetComponentInParent<Canvas>();

            if (_tooltip && CanCreateTooltip())
            {
                ClearTooltip();
            }

            if (!_tooltip && CanCreateTooltip())
            {
                _tooltip = Instantiate(_tooltipPrefab, parentCanvas.transform);
            }

            if (_tooltip)
            {
                UpdateTooltip(_tooltip);
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
            _tooltip.GetComponent<RectTransform>().GetWorldCorners(tooltipCorners);

            // Get inventory slot corners
            var slotCorners = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(slotCorners);

            // Get relative position of the inventory slot
            bool below = transform.position.y < (Screen.height / 2);
            bool right = transform.position.x > (Screen.width / 2);

            // Get corner slotIndex
            int slotCornerIndex = GetCornerIndex(!below, !right);
            int tooltipCornerIndex = GetCornerIndex(below, right);

            // Set tooltip position
            Vector3 moveVector = slotCorners[slotCornerIndex] - tooltipCorners[tooltipCornerIndex];
            _tooltip.transform.position += moveVector;
        }

        private int GetCornerIndex(bool below, bool right)
        {
            if (below && !right)
            {
                return 0;
            }
            else if (!below && !right)
            {
                return 1;
            }
            else if (!below && right)
            {
                return 2;
            }

            return 3;
        }

        private void ClearTooltip()
        {
            if (_tooltip)
            {
                Destroy(_tooltip);
            }
        }
    }
}
