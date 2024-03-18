using UnityEngine;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Inventories;
using RPG.Stats;

namespace RPG.Combat
{
    [CreateAssetMenu(menuName = "ScriptableObject/InventoryItem/WeaponSO")]
    public class WeaponSO : EquipableItemSO, IModifierProvider
    {
        // Variables

        private const string WEAPON = "weapon";

        [SerializeField]
        private Weapon _weaponToAttach;
        [SerializeField]
        private Projectile _projectile;
        [SerializeField]
        private AnimatorOverrideController _animatorOverrideController;
        [SerializeField]
        private bool _isRightHandWeapon;
        [SerializeField]
        private float _weaponRange;
        [SerializeField]
        private float _additiveDamage;
        [SerializeField]
        private float _percentageDamage;

        // Properties

        public float WeaponRange => _weaponRange;
        public float AdditiveDamage => _additiveDamage;
        public float PercentageDamage => _percentageDamage;
        public bool HasProjectTile => _projectile != null;


        // Methods

        /// <summary>
        /// Attach weapon to the hand and set AnimatorOverrideController
        /// </summary>
        /// <param name="rightHandTransform"></param>
        /// <param name="leftHandTransform"></param>
        /// <param name="animator"></param>
        /// <returns></returns>
        public Weapon AttachWeaponToHand(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {
            // Destroy old weapon attached to the hand
            DestroyOldWeapon(rightHandTransform, leftHandTransform);

            // Attach weapon to the hand
            Weapon equippedWeapon = null;

            if (_weaponToAttach != null)
            {
                equippedWeapon = Instantiate(_weaponToAttach, GetHoldingWeaponHandTransform(rightHandTransform, leftHandTransform));
                equippedWeapon.gameObject.name = WEAPON;
            }

            // Set animatorOverrideController
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (_animatorOverrideController != null)
            {
                animator.runtimeAnimatorController = _animatorOverrideController;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return equippedWeapon;
        }

        public void LaunchProjectile(Transform rightHandTransform, Transform leftHandTransform, Health targetHealth, GameObject instigator, float calculatedDamage)
        {
            Projectile projecttileInstance = Instantiate(_projectile, GetHoldingWeaponHandTransform(rightHandTransform, leftHandTransform).position, Quaternion.identity);
            projecttileInstance.SetTarget(instigator, calculatedDamage, targetHealth);
        }

        private void DestroyOldWeapon(Transform rightHandTransform, Transform leftHandTransform)
        {
            Transform oldWeapon = rightHandTransform.Find(WEAPON);

            if (oldWeapon == null)
            {
                oldWeapon = leftHandTransform.Find(WEAPON);
            }

            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";

            Destroy(oldWeapon.gameObject);
        }

        private Transform GetHoldingWeaponHandTransform(Transform rightHandTransform, Transform leftHandTransform)
        {
            if (_isRightHandWeapon)
            {
                return rightHandTransform;
            }

            return leftHandTransform;
        }

        #region IModifierProvider implements
        IEnumerable<float> IModifierProvider.GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _additiveDamage;
            }
        }

        IEnumerable<float> IModifierProvider.GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _percentageDamage;
            }
        }
        #endregion
    }
}
