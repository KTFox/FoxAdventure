using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using RPG.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes {
    public class Health : MonoBehaviour, ISaveable {

        #region Animation strings
        private const string DEATH = "death";
        #endregion

        [SerializeField]
        private UnityEvent<float> OnTakeDamage;

        private LazyValue<float> currentHealth;
        private bool isDeath;

        private void Awake() {
            currentHealth = new LazyValue<float>(GetInitialHealth);
        }

        float GetInitialHealth() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void OnEnable() {
            GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;
        }

        void RegenerateHealth() {
            currentHealth.Value = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start() {
            currentHealth.ForceInit();
        }

        private void OnDisable() {
            GetComponent<BaseStats>().OnLevelUp -= RegenerateHealth;
        }

        public void TakeDamage(GameObject instigator, float damage) {
            currentHealth.Value = Mathf.Max(currentHealth.Value - damage, 0f);

            OnTakeDamage?.Invoke(damage);

            if (currentHealth.Value == 0f) {
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
            return currentHealth.Value;
        }

        public float GetHealthPercentage() {
            return (currentHealth.Value / GetComponent<BaseStats>().GetStat(Stat.Health)) * 100;
        }

        public float GetMaxHealth() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        #region ISaveable interface implements
        public object CaptureState() {
            return currentHealth.Value;
        }

        public void RestoreState(object state) {
            currentHealth.Value = (float)state;

            if (currentHealth.Value <= 0f) {
                Die();
            }
        }
        #endregion
    }
}
