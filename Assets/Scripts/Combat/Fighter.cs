using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction {
        private const string ATTACK = "attack";

        [SerializeField] private float weaponRange = 5f;

        private ActionScheduler actionScheduler;
        private Animator animator;
        private Mover mover;
        private Transform target;

        private void Awake() {
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();
        }

        private void Update() {
            if (target == null) return;

            if (!GetIsInRange()) {
                mover.MoveTo(target.position);
            } else {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        private bool GetIsInRange() {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget) {
            actionScheduler.StartAction(this);
            target = combatTarget.transform;
        }

        private void AttackBehaviour() {
            animator.SetTrigger(ATTACK);
        }

        public void Cancel() {
            target = null;
        }

        // Animation events
        public void Hit() {
            Debug.Log("Hit");
        }
    }
}