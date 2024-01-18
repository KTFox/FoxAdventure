using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes {
    public class Health : MonoBehaviour, ISaveable {

        private const string DEATH = "death";

        [SerializeField]
        private float currentHealth;

        private bool isDeath;

        private void Start() {
            //Maybe conflict with Loading system
            currentHealth = GetComponent<BaseStats>().GetHealth();
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
            GetComponent<Animator>().SetTrigger(DEATH);
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public bool IsDeath() {
            return isDeath;
        }

        #region ISaveable interface implements
        public object CaptureState() {
            return currentHealth;
        }

        public void RestoreState(object state) {
            currentHealth = (float)state;

            if (currentHealth <= 0f) {
                Die();
            }
        }
        #endregion
    }
}
