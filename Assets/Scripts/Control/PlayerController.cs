using UnityEngine;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control {
    public class PlayerController : MonoBehaviour {
        private Mover mover;
        private Fighter fighter;

        private void Awake() {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
        }

        private void Update() {
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;

            print("Nothing to do");
        }

        private bool InteractWithCombat() {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits) {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue; // Skip all the rest of the body and go on to the next hit

                if (Input.GetMouseButtonDown(1)) {
                    fighter.Attack(target);
                }

                return true;
            }

            return false;
        }

        private bool InteractWithMovement() {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit) {
                if (Input.GetMouseButton(1)) {
                    mover.MoveTo(hit.point);
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