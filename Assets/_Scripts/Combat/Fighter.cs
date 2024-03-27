using UnityEngine;
using System.Collections.Generic;
using RPG.Movement;
using RPG.Core;
using RPG.Attributes;
using RPG.Stats;
using RPG.Utility;
using RPG.Inventories;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        // Variables

        private const string ATTACK = "attack";
        private const string STOPATTACK = "stopAttack";

        [SerializeField]
        private float _attackSpeed;
        [SerializeField]
        private WeaponSO _defaultWeaponSO;
        [SerializeField]
        private Transform _rightHandTransform;
        [SerializeField]
        private Transform _lefttHandTransform;
        [SerializeField]
        private float _autoAttackRange = 4f;

        private Mover _mover;
        private Health _targetHealth;
        private Equipment _equipment;
        private WeaponSO _currentWeaponSO;
        private LazyValue<Weapon> _currentWeapon;
        private float _timeSinceLastAttack = Mathf.Infinity;


        // Methods

        private void Awake()
        {
            _mover = GetComponent<Mover>();

            _equipment = GetComponent<Equipment>();
            if (_equipment != null)
            {
                _equipment.OnEquipmentUpdated += Equipment_EquipmentUpdated;
            }

            _currentWeaponSO = _defaultWeaponSO;
            _currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        Weapon SetupDefaultWeapon()
        {
            return AttachWeaponIntoHand(_defaultWeaponSO);
        }

        private void Start()
        {
            _currentWeapon.ForceInit();
        }

        void Equipment_EquipmentUpdated()
        {
            var weapon = _equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponSO;

            if (weapon == null)
            {
                SetupWeapon(_defaultWeaponSO);
            }
            else
            {
                SetupWeapon(weapon);
            }
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (_targetHealth == null)
            {
                return;
            }

            if (_targetHealth.IsDead)
            {
                _targetHealth = FindNewTargetInRange();

                if (_targetHealth == null)
                {
                    return;
                }
            }

            if (!IsTargetInAttackRange(_targetHealth.transform))
            {
                _mover.MoveTo(_targetHealth.transform.position, 1f);
            }
            else
            {
                _mover.Cancel();
                AttackBehaviour();
            }
        }

        /// <summary>
        /// Set _currentWeaponSO and _currentWepon. 
        /// In addition, attach weapon into hand
        /// </summary>
        /// <param name="weaponSO"></param>
        public void SetupWeapon(WeaponSO weaponSO)
        {
            _currentWeaponSO = weaponSO;
            _currentWeapon.Value = AttachWeaponIntoHand(weaponSO);
        }

        /// <summary>
        /// Call StartAction method from ActionScheduler and set _targetHealth
        /// </summary>
        /// <param name="targetObject"></param>
        public void StartAttackAction(GameObject targetObject)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _targetHealth = targetObject.GetComponent<Health>();
        }

        public bool CanAttack(GameObject target)
        {
            if (target == null)
            {
                return false;
            }

            if (!_mover.CanMoveTo(target.transform.position) && !IsTargetInAttackRange(target.transform))
            {
                return false;
            }

            var targetHealth = target.GetComponent<Health>();

            return targetHealth != null && !targetHealth.IsDead;
        }

        public Vector3 GetHandPosition(bool isRightHand)
        {
            if (isRightHand)
            {
                return _rightHandTransform.position;
            }

            return _lefttHandTransform.position;
        }

        private Weapon AttachWeaponIntoHand(WeaponSO weaponSO)
        {
            Animator animator = GetComponent<Animator>();
            return weaponSO.AttachWeaponToHand(_rightHandTransform, _lefttHandTransform, animator);
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_targetHealth.transform);

            if (_timeSinceLastAttack > _attackSpeed)
            {
                TriggerAttack();
                _timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger(STOPATTACK);
            GetComponent<Animator>().SetTrigger(ATTACK);
        }

        private bool IsTargetInAttackRange(Transform targetTransform)
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
            return distanceToTarget < _currentWeaponSO.WeaponRange;
        }

        private Health FindNewTargetInRange()
        {
            Health target = null;
            float bestDistance = Mathf.Infinity;

            foreach (var candidate in FindAllTargetsInRange())
            {
                float distanceToTarget = Vector3.Distance(transform.position, candidate.transform.position);

                if (distanceToTarget < bestDistance)
                {
                    bestDistance = distanceToTarget;
                    target = candidate;
                }
            }

            return target;
        }

        private IEnumerable<Health> FindAllTargetsInRange()
        {
            RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position, _autoAttackRange, Vector3.up);

            foreach (RaycastHit hit in raycastHits)
            {
                var health = hit.transform.GetComponent<Health>();

                if (health == null) continue;
                if (health.IsDead) continue;
                if (health.gameObject == gameObject) continue;

                yield return health;
            }
        }

        #region IAction interface implements
        public void Cancel()
        {
            StopAttack();
            _targetHealth = null;
            _mover.Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger(ATTACK);
            GetComponent<Animator>().SetTrigger(STOPATTACK);
        }
        #endregion

        #region Animation Events
        private void Hit()
        {
            if (_targetHealth == null) return;

            if (_currentWeapon.Value != null)
            {
                _currentWeapon.Value.OnHit();
            }

            float damage = GetComponent<BaseStats>().GetValueOfStat(Stat.Damage);
            BaseStats targetBaseStats = _targetHealth.GetComponent<BaseStats>();

            if (targetBaseStats != null)
            {
                float targetDefence = targetBaseStats.GetValueOfStat(Stat.Defence);
                damage /= 1 + targetDefence / damage;
            }

            if (!_currentWeaponSO.HasProjectTile)
            {
                _targetHealth.TakeDamage(gameObject, damage);
            }
            else
            {
                _currentWeaponSO.LaunchProjectile(_rightHandTransform, _lefttHandTransform, _targetHealth, gameObject, damage);
            }
        }

        private void Shoot()
        {
            Hit();
        }
        #endregion
    }
}