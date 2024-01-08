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
            InteractWithMovement();
            InteractWithCombat();
        }

        private void InteractWithCombat() {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits) {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue; // Skip all the rest of the body and go on to the next hit

                if (Input.GetMouseButtonDown(1)) {
                    fighter.Attack(target);
                }
            }
        }

        private void InteractWithMovement() {
            if (Input.GetMouseButton(1)) {
                MoveToCursor();
            }
        }

        private void MoveToCursor() {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit) {
                mover.MoveTo(hit.point);
            }
        }

        private static Ray GetMouseRay() {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

