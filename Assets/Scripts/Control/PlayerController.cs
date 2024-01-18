using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;

namespace RPG.Control {
    public class PlayerController : MonoBehaviour {

        private Mover mover;
        private Fighter fighter;
        private Health health;

        private void Awake() {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();    
        }

        private void Update() {
            if (health.IsDeath()) return;

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
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                if (target == null) continue;

                if (!fighter.CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButton(1)) {
                    fighter.StartAttackAction(target.gameObject);
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
                    mover.StartMoveAction(hit.point, 1f);
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