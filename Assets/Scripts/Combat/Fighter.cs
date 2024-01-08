using UnityEngine;
using RPG.Movement;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour {
        [SerializeField] private float weaponRange = 5f;

        private Transform target;
        private Mover mover;

        private void Awake() {
            mover = GetComponent<Mover>();
        }

        private void Update() {
            if (target == null) return;

            if (!GetIsInRange()) {
                mover.MoveTo(target.position);
            } else {
                mover.Stop();
            }
        }

        private bool GetIsInRange() {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget) {
            target = combatTarget.transform;
        }

        public void Cancel() {
            target = null;
        }
    }
}