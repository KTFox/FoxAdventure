using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat {
    [CreateAssetMenu(fileName = "New WeaponSO", menuName = "Combat/Create new WeaponSO", order = 0)]
    public class WeaponSO : ScriptableObject {

        private const string weaponName = "weapon";

        [SerializeField] 
        private Weapon equippedPrefab;
        [SerializeField] 
        private Projectile projectile;
        [SerializeField] 
        private AnimatorOverrideController animatorOverrideController;
        [SerializeField] 
        private bool rightHandWeapon;
        [SerializeField] 
        private float _weaponRange;
        [SerializeField] 
        private float _weaponDamage;
        [SerializeField] 
        private float _percentageBonus;

        #region Properties
        public float WeaponRange {
            get {
                return _weaponRange;
            }
        }

        public float WeaponDamage {
            get {
                return _weaponDamage;
            }
        }

        public float PercentageBonus {
            get {
                return _percentageBonus;
            }
        }
        #endregion

        /// <summary>
        /// Attach weapon to the hand and set AnimatorOverrideController
        /// </summary>
        /// <param name="rightHandTransform"></param>
        /// <param name="leftHandTransform"></param>
        /// <param name="animator"></param>
        /// <returns></returns>
        public Weapon Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator) {
            //Destroy old weapon attached to the hand
            DestroyOldWeapon(rightHandTransform, leftHandTransform);

            //Attach weapon to the hand
            Weapon equippedWeapon = null;
            if (equippedPrefab != null) {
                equippedWeapon = Instantiate(equippedPrefab, GetHandTransform(rightHandTransform, leftHandTransform));
                equippedWeapon.gameObject.name = weaponName;
            }

            //Set overrideController
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverrideController != null) {
                animator.runtimeAnimatorController = animatorOverrideController;
            } else if (overrideController != null) {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return equippedWeapon;
        }

        private void DestroyOldWeapon(Transform rightHandTransform, Transform leftHandTransform) {
            Transform oldWeapon = rightHandTransform.Find(weaponName);
            if (oldWeapon == null) {
                //Old weapon is not be placed on right hand

                oldWeapon = leftHandTransform.Find(weaponName);
            }

            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        /// <summary>
        /// Spawn projectitle and set target
        /// </summary>
        /// <param name="rightHandTransform"></param>
        /// <param name="leftHandTransform"></param>
        /// <param name="target"></param>
        /// <param name="instigator"></param>
        /// <param name="calculatedDamage"></param>
        public void LaunchProjectile(Transform rightHandTransform, Transform leftHandTransform, Health target, GameObject instigator, float calculatedDamage) {
            Projectile projecttileInstance = Instantiate(projectile, GetHandTransform(rightHandTransform, leftHandTransform).position, Quaternion.identity);
            projecttileInstance.SetTarget(target, instigator, calculatedDamage);
        }

        public bool HasProjectile() {
            return projectile != null;
        }

        private Transform GetHandTransform(Transform rightHandTransform, Transform leftHandTransform) {
            Transform handTransform;

            if (rightHandWeapon) {
                handTransform = rightHandTransform;
            } else {
                handTransform = leftHandTransform;
            }

            return handTransform;
        }
    }
}