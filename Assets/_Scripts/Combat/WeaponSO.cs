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
        private float weaponRange;
        [SerializeField]
        private float weaponDamage;
        [SerializeField]
        private float percentageBonus;

        public Weapon Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator) {
            DestroyOldWeapon(rightHandTransform, leftHandTransform);

            Weapon equippedWeapon = null;
            if (equippedPrefab != null) {
                equippedWeapon = Instantiate(equippedPrefab, GetHandTransform(rightHandTransform, leftHandTransform));
                equippedWeapon.gameObject.name = weaponName;
            }

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
                oldWeapon = leftHandTransform.Find(weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

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

        public float GetWeaponRange() {
            return weaponRange;
        }

        public float GetWeaponDamage() {
            return weaponDamage;
        }

        public float GetPercentageBonus() {
            return percentageBonus;
        }
    }
}
