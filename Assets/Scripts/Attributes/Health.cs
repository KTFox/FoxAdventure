using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes {
    public class Health : MonoBehaviour, ISaveable {

        #region Animation strings
        private const string DEATH = "death";
        #endregion

        private float currentHealth = -1f;
        private bool isDeath;

        private void Start() {
            GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;

            if (currentHealth < 0f) {
                //There's no save file
                currentHealth = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        private void RegenerateHealth() {
            currentHealth = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void TakeDamage(GameObject instigator, float damage) {
            Debug.Log($"{gameObject.name} took {damage} damage");

            currentHealth = Mathf.Max(currentHealth - damage, 0f);

            if (currentHealth == 0f) {
                Die();
                AwardExperience(instigator);
            }
        }

        private void Die() {
            if (IsDeath()) return;

            isDeath = true;
            GetComponent<Animator>().SetTrigger(DEATH);
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        /// <summary>
        /// Gain instigator's experience 
        /// </summary>
        /// <param name="instigator"></param>
        private void AwardExperience(GameObject instigator) {
            Experience instigatorExperience = instigator.GetComponent<Experience>();
            if (instigatorExperience == null) return;

            instigatorExperience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public bool IsDeath() {
            return isDeath;
        }

        public float GetCurrentHealth() {
            return currentHealth;
        }

        public float GetHealthPercentage() {
            return (currentHealth / GetComponent<BaseStats>().GetStat(Stat.Health)) * 100;
        }

        public float GetMaxHealth() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
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
