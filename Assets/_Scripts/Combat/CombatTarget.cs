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
            var fighter = playerController.GetComponent<Fighter>();

            if (!fighter.CanAttack(gameObject))
            {
                return false;
            }

            if (Input.GetMouseButton(1))
            {
                fighter.StartAttackAction(gameObject);
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