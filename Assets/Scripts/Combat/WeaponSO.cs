using RPG.Core;
using System;
using UnityEngine;

namespace RPG.Combat {
    [CreateAssetMenu(fileName = "New weapon SO", menuName = "Create new weapon SO")]
    public class WeaponSO : ScriptableObject {

        private const string weaponName = "weapon";

        [SerializeField]
        private GameObject equippedPrefab;
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
            DestroyOldWeapon(rightHandTransform, leftHandTransform);

            if (equippedPrefab != null) {
                GameObject equippedWeapon = Instantiate(equippedPrefab, GetHandTransform(rightHandTransform, leftHandTransform));
                equippedWeapon.name = weaponName;
            }

            if (animatorOverrideController != null) {
                animator.runtimeAnimatorController = animatorOverrideController;
            }
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
