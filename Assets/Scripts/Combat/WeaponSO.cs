using RPG.Core;
using UnityEngine;

namespace RPG.Combat {
    [CreateAssetMenu(fileName = "New weapon SO", menuName = "Create new weapon SO")]
    public class WeaponSO : ScriptableObject {

        [SerializeField]
        private GameObject weaponPrefab;
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

        public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator) {
            if (weaponPrefab != null) {
                Instantiate(weaponPrefab, GetHandTransform(rightHandTransform, leftHandTransform));
            }

            if (animatorOverrideController != null) {
                animator.runtimeAnimatorController = animatorOverrideController;
            }
        }

        public void LaunchProjectile(Transform rightHandTransform, Transform leftHandTransform, Health target) {
            Projectile projecttileInstance = Instantiate(projectile, GetHandTransform(rightHandTransform, leftHandTransform).position, Quaternion.identity);
            projecttileInstance.SetTarget(target, weaponDamage);
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
    }
}
