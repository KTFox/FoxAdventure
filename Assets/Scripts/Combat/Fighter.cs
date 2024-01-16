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
        private float timeSinceLastAttack = Mathf.Infinity;

        [SerializeField]
        private float weaponDamage;
        [SerializeField]
        private GameObject weaponPrefab = null;
        [SerializeField]
        private Transform handTransform = null;
        [SerializeField]
        private AnimatorOverrideController animatorOverrideController = null;

        private ActionScheduler actionScheduler;
        private Animator animator;
        private Mover mover;
        private Health targetHealth;

        private void Awake() {
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();
        }

        private void Start() {
            SpawnWeapon();
        }

        private void SpawnWeapon() {
            Instantiate(weaponPrefab, handTransform);
            animator.runtimeAnimatorController = animatorOverrideController;
        }

        private void Update() {
            timeSinceLastAttack += Time.deltaTime;

            if (targetHealth == null) return;
            if (targetHealth.IsDeath()) return;

            if (!GetIsInRange()) {
                mover.MoveTo(targetHealth.transform.position, 1f);
            } else {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        private bool GetIsInRange() {
            float distanceToTarget = Vector3.Distance(transform.position, targetHealth.transform.position);
            return distanceToTarget < weaponRange;
        }

        private void AttackBehaviour() {
            transform.LookAt(targetHealth.transform);

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
        /// return true if targetObject is not null and !targetObject.Health.IsDeath()
        /// </summary>
        /// <param name="targetObject"></param>
        /// <returns></returns>
        public bool CanAttack(GameObject targetObject) {
            if (targetObject == null) {
                return false;
            }

            Health targetToTest = targetObject.GetComponent<Health>();

            return targetToTest != null && !targetToTest.IsDeath();
        }

        /// <summary>
        /// Call actionScheduler.StartAction() and Set fighter.targetHealth equal targetObject
        /// </summary>
        /// <param name="targetObject"></param>
        public void StartAttackAction(GameObject targetObject) {
            actionScheduler.StartAction(this);
            targetHealth = targetObject.GetComponent<Health>();
        }

        #region IAction interface implements
        public void Cancel() {
            animator.SetTrigger(STOPATTACK);
            targetHealth = null;
            mover.Cancel();
        }
        #endregion

        #region Animation Events
        public void Hit() {
            if (targetHealth == null) return;

            targetHealth.TakeDamage(weaponDamage);
        }
        #endregion
    }
}