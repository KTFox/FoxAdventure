using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Utility.UI
{
    public class DragItem<T> : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler where T : class
    {
        // Variables

        private IDragSource<T> _source;
        private Canvas _parentCanvas;
        private Vector3 _startPosition;
        private Transform _originalParent;


        // Methods

        private void Awake()
        {
            _source = GetComponentInParent<IDragSource<T>>();
            _parentCanvas = GetComponentInParent<Canvas>();
        }

        #region IDragHandler implements
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            _startPosition = transform.position;
            _originalParent = transform.parent;

            GetComponent<CanvasGroup>().blocksRaycasts = false;

            transform.SetParent(_parentCanvas.transform, true);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            transform.position = _startPosition;
            transform.SetParent(_originalParent, true);

            GetComponent<CanvasGroup>().blocksRaycasts = true;

            IDragDestination<T> container;

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // Drop item into the world

                container = _parentCanvas.GetComponent<IDragDestination<T>>();
            }
            else
            {
                container = GetContainer(eventData);
            }

            // Drop item into destination container
            if (container != null)
            {
                DropItemIntoContainer(container);
            }
        }
        #endregion

        private IDragDestination<T> GetContainer(PointerEventData eventData)
        {
            if (eventData.pointerEnter != null)
            {
                var container = eventData.pointerEnter.GetComponentInParent<IDragDestination<T>>();

                return container;
            }

            return null;
        }

        private void DropItemIntoContainer(IDragDestination<T> destination)
        {
            if (ReferenceEquals(destination, _source)) return;

            var destinationContainer = destination as IDragContainer<T>;
            var sourceContainer = _source as IDragContainer<T>;

            if (destinationContainer == null || sourceContainer == null || ReferenceEquals(destinationContainer.InventoryItemSO, sourceContainer.InventoryItemSO))
            {
                AttempSimpleTransfer(destination);

                return;
            }

            AttempSwap(destinationContainer, sourceContainer);
        }

        private void AttempSimpleTransfer(IDragDestination<T> destination)
        {
            T draggingItem = _source.InventoryItemSO;

            int draggingQuantity = _source.ItemQuantity;
            int acceptableQuantity = destination.GetMaxAcceptable(draggingItem);
            int quantityToTransfer = Mathf.Min(draggingQuantity, acceptableQuantity);

            if (quantityToTransfer > 0)
            {
                _source.RemoveItems(quantityToTransfer);
                destination.AddItems(draggingItem, quantityToTransfer);
            }
        }

        private void AttempSwap(IDragContainer<T> destination, IDragContainer<T> source)
        {
            T removedItemFromSource = source.InventoryItemSO;
            T removedItemFromDestination = destination.InventoryItemSO;
            int removedItemNumberFromSource = source.ItemQuantity;
            int removedItemNumberFromDestination = destination.ItemQuantity;

            // Provisionally remove items from both sides

            source.RemoveItems(removedItemNumberFromSource);
            destination.RemoveItems(removedItemNumberFromDestination);

            // Calculate take back quantity from both sides

            int sourceTakeBackNumber = CalculateTakeBackNumber(removedItemFromSource, removedItemNumberFromSource, source, destination);
            int destinationTakeBackNumber = CalculateTakeBackNumber(removedItemFromDestination, removedItemNumberFromDestination, destination, source);

            // Do take back (if needed)

            if (sourceTakeBackNumber > 0)
            {
                source.AddItems(removedItemFromSource, sourceTakeBackNumber);
                removedItemNumberFromSource -= sourceTakeBackNumber;
            }

            if (destinationTakeBackNumber > 0)
            {
                destination.AddItems(removedItemFromDestination, destinationTakeBackNumber);
                removedItemNumberFromDestination -= destinationTakeBackNumber;
            }

            // Abort if fail to swap

            if (source.GetMaxAcceptable(removedItemFromDestination) < removedItemNumberFromDestination ||
                destination.GetMaxAcceptable(removedItemFromSource) < removedItemNumberFromSource)
            {
                source.AddItems(removedItemFromSource, removedItemNumberFromSource);
                destination.AddItems(removedItemFromDestination, removedItemNumberFromDestination);

                return;
            }

            // Do swap

            if (removedItemNumberFromDestination > 0)
            {
                source.AddItems(removedItemFromDestination, removedItemNumberFromDestination);
            }

            if (removedItemNumberFromSource > 0)
            {
                destination.AddItems(removedItemFromSource, removedItemNumberFromSource);
            }
        }

        private int CalculateTakeBackNumber(T item, int removedNumber, IDragContainer<T> removeSource, IDragContainer<T> destination)
        {
            int takeBackNumber = 0;
            int destinationMaxAcceptable = destination.GetMaxAcceptable(item);

            if (destinationMaxAcceptable < removedNumber)
            {
                takeBackNumber = removedNumber - destinationMaxAcceptable;
                int sourceMaxAcceptable = removeSource.GetMaxAcceptable(item);

                if (sourceMaxAcceptable < takeBackNumber)
                {
                    return 0;
                }
            }

            return takeBackNumber;
        }
    }
}
