using UnityEngine;
using RPG.Inventories;

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
        CursorType IRaycastable.GetCursorType()
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

        bool IRaycastable.HandleRaycast(PlayerController callingController)
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
