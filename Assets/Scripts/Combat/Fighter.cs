using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction {
        private const string ATTACK = "attack";
        private const string STOPATTACK = "stopAttack";

        [SerializeField] private float weaponRange = 5f;
        [SerializeField] private float timeBetweenAttacks = 2f;
        [SerializeField] private float weaponDamage;

        private ActionScheduler actionScheduler;
        private Animator animator;
        private Mover mover;
        private Health target;
        private float timeSinceLastAttack = 0f;

        private void Awake() {
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();
        }

        private void Update() {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDeath()) return;

            if (!GetIsInRange()) {
                mover.MoveTo(target.transform.position);
            } else {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        public void Attack(CombatTarget combatTarget) {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        private void AttackBehaviour() {
            transform.LookAt(target.transform);

            if (timeSinceLastAttack > timeBetweenAttacks) {
                TriggerAttack();
                timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack() {
            animator.ResetTrigger(STOPATTACK);

            // This will trigger Hit event
            animator.SetTrigger(ATTACK);
        }

        public void Cancel() {
            animator.SetTrigger(STOPATTACK);
            target = null;
        }

        private bool GetIsInRange() {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public bool CanAttack(CombatTarget combatTarget) {
            if (combatTarget == null) {
                return false;
            }

            Health targetHealth = combatTarget.GetComponent<Health>();
            return targetHealth != null && !targetHealth.IsDeath();
        }



        // Animation events
        public void Hit() {
            if (target == null) return;

            target.TakeDamage(weaponDamage);
        }
    }
}