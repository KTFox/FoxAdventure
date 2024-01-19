using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes {
    public class Health : MonoBehaviour, ISaveable {

        #region Animation strings
        private const string DEATH = "death";
        #endregion

        private LazyValue<float> currentHealth;
        private bool isDeath;

        private void Awake() {
            currentHealth = new LazyValue<float>(GetIntialHealth);
        }

        float GetIntialHealth() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void OnEnable() {
            GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;
        }

        void RegenerateHealth() {
            currentHealth.value = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start() {
            currentHealth.ForceInit();
        }

        private void OnDisable() {
            GetComponent<BaseStats>().OnLevelUp -= RegenerateHealth;
        }

        public void TakeDamage(GameObject instigator, float damage) {
            Debug.Log($"{gameObject.name} took {damage} damage");

            currentHealth.value = Mathf.Max(currentHealth.value - damage, 0f);

            if (currentHealth.value == 0f) {
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
            return currentHealth.value;
        }

        public float GetHealthPercentage() {
            return (currentHealth.value / GetComponent<BaseStats>().GetStat(Stat.Health)) * 100;
        }

        public float GetMaxHealth() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        #region ISaveable interface implements
        public object CaptureState() {
            return currentHealth;
        }

        public void RestoreState(object state) {
            currentHealth.value = (float)state;

            if (currentHealth.value <= 0f) {
                Die();
            }
        }
        #endregion
    }
}
