using UnityEngine;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control {
    public class PlayerController : MonoBehaviour {

        private Mover mover;
        private Fighter fighter;

        private void Start() {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
        }

        private void Update() {
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        /// <summary>
        /// Return true if combatTarget is not null and fighter.CanAttack(combatTarget)
        /// </summary>
        /// <returns></returns>
        private bool InteractWithCombat() {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits) {
                CombatTarget combatTarget = hit.transform.GetComponent<CombatTarget>();

                if (!fighter.CanAttack(combatTarget)) continue; 

                if (Input.GetMouseButtonDown(1)) {
                    fighter.Attack(combatTarget);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Return Physics.Raycast(GetMouseRay())
        /// </summary>
        /// <returns></returns>
        private bool InteractWithMovement() {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit) {
                if (Input.GetMouseButton(1)) {
                    mover.StartMoveAction(hit.point);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay() {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}