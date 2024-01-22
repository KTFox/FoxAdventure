using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using RPG.Utility;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider {

        #region Animation strings
        private const string ATTACK = "attack";
        private const string STOPATTACK = "stopAttack";
        #endregion

        [SerializeField]
        private float timeBetweenAttacks;
        private float timeSinceLastAttack = Mathf.Infinity;

        [SerializeField]
        private WeaponSO defaultWeaponSO;
        private LazyValue<WeaponSO> currentWeaonSO;
        [SerializeField]
        private Transform rightHandTransform = null;
        [SerializeField]
        private Transform lefttHandTransform = null;

        private ActionScheduler actionScheduler;
        private Animator animator;
        private Mover mover;
        private Health targetHealth;

        private void Awake() {
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();

            currentWeaonSO = new LazyValue<WeaponSO>(SetupDefaultWeapon);
        }

        WeaponSO SetupDefaultWeapon() {
            defaultWeaponSO.Spawn(rightHandTransform, lefttHandTransform, animator);
            return defaultWeaponSO;
        }

        private void Start() {
            currentWeaonSO.ForceInit();
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

        public void EquipWeapon(WeaponSO weaponSO) {
            weaponSO.Spawn(rightHandTransform, lefttHandTransform, animator);
            currentWeaonSO.Value = weaponSO;
        }

        private bool GetIsInRange() {
            float distanceToTarget = Vector3.Distance(transform.position, targetHealth.transform.position);
            return distanceToTarget < currentWeaonSO.Value.GetWeaponRange();
        }

        public Health GetTargetHealth() {
            return targetHealth;
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

        #region ISaveable interface implements
        public object CaptureState() {
            return currentWeaonSO.Value.name;
        }

        public void RestoreState(object state) {
            string weaponName = (string)state;
            WeaponSO weapon = Resources.Load<WeaponSO>($"ScriptableObject/WeaponSO/{weaponName}");

            EquipWeapon(weapon);
        }
        #endregion

        #region IModifierProvider implements
        public IEnumerable<float> GetAdditiveModifiers(Stat stat) {
            if (stat == Stat.Damage) {
                yield return currentWeaonSO.Value.GetWeaponDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat) {
            if (stat == Stat.Damage) {
                yield return currentWeaonSO.Value.GetPercentageBonus();
            }
        }
        #endregion

        #region Animation Events
        private void Hit() {
            if (targetHealth == null) return;

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (!currentWeaonSO.Value.HasProjectile()) {
                targetHealth.TakeDamage(gameObject, damage);
            } else {
                currentWeaonSO.Value.LaunchProjectile(rightHandTransform, lefttHandTransform, targetHealth, gameObject, damage);
            }
        }

        private void Shoot() {
            Hit();
        }
        #endregion
    }
}