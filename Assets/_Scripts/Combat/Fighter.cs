using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using RPG.Utility;
using RPG.Inventories;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        #region Animation strings
        private const string ATTACK = "attack";
        private const string STOPATTACK = "stopAttack";
        #endregion

        [SerializeField]
        private float timeBetweenAttacks;
        [SerializeField]
        private WeaponSO defaultWeaponSO;
        [SerializeField]
        private Transform rightHandTransform;
        [SerializeField]
        private Transform lefttHandTransform;

        private Mover mover;
        private Health _targetHealth;
        private Equipment equipment;
        private WeaponSO currentWeaponSO;
        private LazyValue<Weapon> currentWeapon;
        private float timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            equipment = GetComponent<Equipment>();

            currentWeaponSO = defaultWeaponSO;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);

            if (equipment)
            {
                equipment.OnEquipmentUpdated += UpdateWeapon;
            }
        }

        Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeaponSO);
        }

        void UpdateWeapon()
        {
            var weapon = equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponSO;
            if (!weapon)
            {
                EquipWeapon(defaultWeaponSO);
            }
            else
            {
                EquipWeapon(weapon);
            }
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (_targetHealth == null) return;
            if (_targetHealth.IsDead) return;

            if (!TargetInAttackRange(_targetHealth.transform))
            {
                //Target out of attack range

                //Move to target
                mover.MoveTo(_targetHealth.transform.position, 1f);
            }
            else
            {
                //Target in attack range

                mover.Cancel();
                AttackBehaviour();
            }
        }

        /// <summary>
        /// Set currentWeaonSO equal weaponSO. Spawn Weapon into handTransform.
        /// </summary>
        /// <param name="weaponSO"></param>
        public void EquipWeapon(WeaponSO weaponSO)
        {
            currentWeaponSO = weaponSO;
            currentWeapon.Value = AttachWeapon(weaponSO);
        }

        private Weapon AttachWeapon(WeaponSO weaponSO)
        {
            Animator animator = GetComponent<Animator>();
            return weaponSO.Spawn(rightHandTransform, lefttHandTransform, animator);
        }

        private bool TargetInAttackRange(Transform targetTransform)
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
            return distanceToTarget < currentWeaponSO.WeaponRange;
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_targetHealth.transform);

            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger(STOPATTACK);
            GetComponent<Animator>().SetTrigger(ATTACK);
        }

        public bool CanAttack(GameObject targetObject)
        {
            if (targetObject == null) return false;

            //Can attack if target in attack range even if fighter cannot move to target
            if (!mover.CanMoveTo(targetObject.transform.position) && !TargetInAttackRange(targetObject.transform)) return false;

            Health targetToTest = targetObject.GetComponent<Health>();

            return targetToTest != null && !targetToTest.IsDead;
        }

        /// <summary>
        /// Call actionScheduler.StartAction() and Set fighter._targetHealth equal targetObject
        /// </summary>
        /// <param name="targetObject"></param>
        public void StartAttackAction(GameObject targetObject)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _targetHealth = targetObject.GetComponent<Health>();
        }

        public Vector3 GetHandTransform (bool isRightHand)
        {
            if (isRightHand)
            {
                return rightHandTransform.position;
            }

            return lefttHandTransform.position;
        }

        #region IAction interface implements
        public void Cancel()
        {
            StopAttack();
            _targetHealth = null;
            mover.Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger(ATTACK);
            GetComponent<Animator>().SetTrigger(STOPATTACK);
        }
        #endregion

        #region ISaveable interface implements
        public object CaptureState()
        {
            return currentWeaponSO.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponSO weapon = Resources.Load<WeaponSO>($"ScriptableObject/WeaponSO/{weaponName}");

            EquipWeapon(weapon);
        }
        #endregion

        #region Animation Events
        private void Hit()
        {
            if (_targetHealth == null) return;

            if (currentWeapon.Value != null)
            {
                currentWeapon.Value.CallHitEvent();
            }

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (!currentWeaponSO.HasProjectile())
            {
                _targetHealth.TakeDamage(gameObject, damage);
            }
            else
            {
                currentWeaponSO.LaunchProjectile(rightHandTransform, lefttHandTransform, _targetHealth, gameObject, damage);
            }
        }

        private void Shoot()
        {
            Hit();
        }
        #endregion
    }
}