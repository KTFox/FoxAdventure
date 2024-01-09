using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour {
        [SerializeField] private float weaponRange = 5f;

        private ActionScheduler actionScheduler;
        private Transform target;
        private Mover mover;

        private void Awake() {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
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
            actionScheduler.StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel() {
            target = null;
        }
    }
}