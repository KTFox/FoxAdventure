using UnityEngine;

namespace RPG.Combat {
    [CreateAssetMenu(fileName = "New weapon SO", menuName = "Create new weapon SO")]
    public class WeaponSO : ScriptableObject {

        [SerializeField]
        private GameObject weaponPrefab = null;
        [SerializeField]
        private AnimatorOverrideController animatorOverrideController = null;

        public void Spawn(Transform handTransform, Animator animator) {
            Instantiate(weaponPrefab, handTransform);
            animator.runtimeAnimatorController = animatorOverrideController;
        }
    }
}
