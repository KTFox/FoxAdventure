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
        private WeaponSO currentWeaonSO;
        private LazyValue<Weapon> currentWeapon;
        [SerializeField]
        private Transform rightHandTransform = null;
        [SerializeField]
        private Transform lefttHandTransform = null;

        private Mover mover;
        private Health targetHealth;

        private void Awake() {
            mover = GetComponent<Mover>();

            currentWeaonSO = defaultWeaponSO;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private Weapon SetupDefaultWeapon() {
            return AttachWeapon(defaultWeaponSO);
        }

        private void Start() {
            currentWeapon.ForceInit();
        }

        private void Update() {
            timeSinceLastAttack += Time.deltaTime;

            if (targetHealth == null) return;
            if (targetHealth.IsDeath()) return;

            if (!GetIsInRange(targetHealth.transform)) {
                mover.MoveTo(targetHealth.transform.position, 1f);
            } else {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(WeaponSO weaponSO) {
            currentWeaonSO = weaponSO;
            currentWeapon.Value = AttachWeapon(weaponSO);
        }

        private Weapon AttachWeapon(WeaponSO weaponSO) {
            Animator animator = GetComponent<Animator>();
            return weaponSO.Spawn(rightHandTransform, lefttHandTransform, animator);
        }

        private bool GetIsInRange(Transform targetTransform) {
            float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
            return distanceToTarget < currentWeaonSO.GetWeaponRange();
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
            GetComponent<Animator>().ResetTrigger(STOPATTACK);
            GetComponent<Animator>().SetTrigger(ATTACK);
        }

        /// <summary>
        /// return true if targetObject is not null and !targetObject.Health.IsDeath()
        /// </summary>
        /// <param name="targetObject"></param>
        /// <returns></returns>
        public bool CanAttack(GameObject targetObject) {
            if (targetObject == null) return false;
            if (!mover.CanMoveTo(targetObject.transform.position) && !GetIsInRange(targetObject.transform)) return false;

            Health targetToTest = targetObject.GetComponent<Health>();

            return targetToTest != null && !targetToTest.IsDeath();
        }

        /// <summary>
        /// Call actionScheduler.StartAction() and Set fighter.targetHealth equal targetObject
        /// </summary>
        /// <param name="targetObject"></param>
        public void StartAttackAction(GameObject targetObject) {
            GetComponent<ActionScheduler>().StartAction(this);
            targetHealth = targetObject.GetComponent<Health>();
        }

        #region IAction interface implements
        public void Cancel() {
            StopAttack();
            targetHealth = null;
            mover.Cancel();
        }

        private void StopAttack() {
            GetComponent<Animator>().ResetTrigger(ATTACK);
            GetComponent<Animator>().SetTrigger(STOPATTACK);
        }
        #endregion

        #region ISaveable interface implements
        public object CaptureState() {
            return currentWeaonSO.name;
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
                yield return currentWeaonSO.GetWeaponDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat) {
            if (stat == Stat.Damage) {
                yield return currentWeaonSO.GetPercentageBonus();
            }
        }
        #endregion

        #region Animation Events
        private void Hit() {
            if (targetHealth == null) return;

            if (currentWeapon.Value != null) {
                currentWeapon.Value.CallHitEvent();
            }

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (!currentWeaonSO.HasProjectile()) {
                targetHealth.TakeDamage(gameObject, damage);
            } else {
                currentWeaonSO.LaunchProjectile(rightHandTransform, lefttHandTransform, targetHealth, gameObject, damage);
            }
        }

        private void Shoot() {
            Hit();
        }
        #endregion
    }
}