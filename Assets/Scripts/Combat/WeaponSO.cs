using UnityEngine;

namespace RPG.Combat {
    [CreateAssetMenu(fileName = "New weapon SO", menuName = "Create new weapon SO")]
    public class WeaponSO : ScriptableObject {

        [SerializeField]
        private GameObject weaponPrefab;
        [SerializeField]
        private AnimatorOverrideController animatorOverrideController;
        [SerializeField]
        private float weaponRange;
        [SerializeField]
        private float weaponDamage;

        public void Spawn(Transform handTransform, Animator animator) {
            if (weaponPrefab != null) {
                Instantiate(weaponPrefab, handTransform);
            }
            if (animatorOverrideController != null) {
                animator.runtimeAnimatorController = animatorOverrideController;
            }
        }

        public float GetWeaponRange() {
            return weaponRange;
        }

        public float GetWeaponDamage() {
            return weaponDamage;
        }
    }
}
