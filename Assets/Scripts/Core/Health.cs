using UnityEngine;

namespace RPG.Core {
    public class Health : MonoBehaviour {

        #region Animation strings
        private const string DEATH = "death";
        #endregion

        [SerializeField]
        private float currentHealth;

        private ActionScheduler actionScheduler;
        private Animator animator;
        private bool isDeath;

        private void Start() {
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }

        public void TakeDamage(float damage) {
            currentHealth = Mathf.Max(currentHealth - damage, 0f);

            if (currentHealth <= 0f) {
                Die();
            }
        }

        private void Die() {
            if (IsDeath()) return;

            isDeath = true;
            animator.SetTrigger(DEATH);
            actionScheduler.CancelCurrentAction();
        }

        public bool IsDeath() {
            return isDeath;
        }
    }
}
