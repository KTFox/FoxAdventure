using UnityEngine;
using RPG.Inventory;

namespace RPG.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        private Pickup pickup;

        private void Awake()
        {
            pickup = GetComponent<Pickup>();
        }

        #region IRaycastable implements
        public CursorType GetCursorType()
        {
            if (pickup.CanBePickedUp)
            {
                return CursorType.Pickup;
            }
            else
            {
                return CursorType.FullInventory;
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(1))
            {
                pickup.PickupItem();
            }

            return true;
        }
        #endregion
    }
}
