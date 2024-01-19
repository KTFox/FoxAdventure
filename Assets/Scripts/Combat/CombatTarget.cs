using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat {
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable {



        #region IRaycastable implements
        public bool HandleRaycast(PlayerController callingController) {
            Fighter callingFighter = callingController.GetComponent<Fighter>();

            if (!callingFighter.CanAttack(gameObject)) {
                return false;
            }

            if (Input.GetMouseButton(1)) {
                callingFighter.StartAttackAction(gameObject);
            }

            return true;
        }
        #endregion
    }
}