using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction {

        #region const string variables
        private const string ATTACK = "attack";
        private const string STOPATTACK = "stopAttack";
        #endregion

        [SerializeField]
        private float weaponRange;

        [SerializeField]
        private float timeBetweenAttacks;
        private float timeSinceLastAttack;

        [SerializeField]
        private float weaponDamage;

        private ActionScheduler actionScheduler;
        private Animator animator;
        private Mover mover;
        private Health target;

        private void Start() {
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

        private bool GetIsInRange() {
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            return distanceToTarget < weaponRange;
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

        /// <summary>
        /// Call actionScheduler.StartAction() and Set fighter.target equal combatTarget
        /// </summary>
        /// <param name="combatTarget"></param>
        public void Attack(CombatTarget combatTarget) {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        /// <summary>
        /// return true if combatTarget is not null and !combatTarget.Health.IsDeath()
        /// </summary>
        /// <param name="combatTarget"></param>
        /// <returns></returns>
        public bool CanAttack(CombatTarget combatTarget) {
            if (combatTarget == null) {
                return false;
            }

            Health targetHealth = combatTarget.GetComponent<Health>();

            return targetHealth != null && !targetHealth.IsDeath();
        }

        #region IAction interface implements
        public void Cancel() {
            animator.SetTrigger(STOPATTACK);
            target = null;
        }
        #endregion

        #region Animation Events
        public void Hit() {
            if (target == null) return;

            target.TakeDamage(weaponDamage);
        }
        #endregion
    }
}