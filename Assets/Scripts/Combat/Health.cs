using UnityEngine;

namespace RPG.Combat {
    public class Health : MonoBehaviour {
        private const string DEATH = "death";

        [SerializeField] private float currentHealth;

        private Animator animator;
        private bool isDeath;

        private void Awake() {
            animator = GetComponent<Animator>();
        }

        public void TakeDamage(float damage) {
            currentHealth = Mathf.Max(currentHealth - damage, 0f);
            Debug.Log($"Current currentHealth: {currentHealth}");

            if (currentHealth <= 0f && !isDeath) {
                Die();
            }
        }

        private void Die() {
            isDeath = true;
            animator.SetTrigger(DEATH);
        }

        public bool IsDeath() {
            return isDeath;
        }
    }
}
