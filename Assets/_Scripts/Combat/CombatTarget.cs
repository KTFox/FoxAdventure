using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        #region IRaycastable implements
        public bool HandleRaycast(PlayerController playerController)
        {
            if (!enabled)
            {
                return false;
            }

            if (!playerController.GetComponent<Fighter>().CanAttack(gameObject))
            {
                return false;
            }

            if (Input.GetMouseButton(1))
            {
                playerController.GetComponent<Fighter>().StartAttackAction(gameObject);
            }

            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }
        #endregion
    }
}