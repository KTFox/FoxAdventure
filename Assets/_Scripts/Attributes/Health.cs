using System;
using UnityEngine;
using UnityEngine.Events;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using RPG.Utility;

namespace RPG.Attributes {
    public class Health : MonoBehaviour, ISaveable {

        [SerializeField] 
        private UnityEvent<float> OnTakeDamage;
        [SerializeField] 
        private UnityEvent OnDie;

        private LazyValue<float> _currentHealth;
        private bool _isDeath;

        #region Properties
        public float CurrentHealth {
            get {
                return _currentHealth.Value;
            }
        }

        public float CurrentHealthFraction {
            get {
                return _currentHealth.Value / GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        public float CurrentHealthPercentage {
            get {
                return CurrentHealthFraction * 100;
            }
        }

        public float MaxHealth {
            get {
                return GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        public bool IsDeath {
            get {
                return _isDeath;
            }
        }
        #endregion

        private void Awake() {
            _currentHealth = new LazyValue<float>(GetInitialHealth);
        }

        float GetInitialHealth() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void OnEnable() {
            GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;
        }

        void RegenerateHealth() {
            _currentHealth.Value = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start() {
            _currentHealth.ForceInit();
        }

        private void OnDisable() {
            GetComponent<BaseStats>().OnLevelUp -= RegenerateHealth;
        }

        /// <summary>
        /// Reduce _currentHealth by an amount equal to damage.
        /// Callback OnDie() and OnTakeDamage() event.
        /// </summary>
        /// <param name="instigator"></param>
        /// <param name="damage"></param>
        public void TakeDamage(GameObject instigator, float damage) {
            _currentHealth.Value = Mathf.Max(_currentHealth.Value - damage, 0f);

            if (_currentHealth.Value == 0f) {
                Die();
                AwardExperience(instigator);
                OnDie?.Invoke();
            } else {
                OnTakeDamage?.Invoke(damage);
            }
        }

        private void Die() {
            if (_isDeath) return;

            _isDeath = true;
            GetComponent<Animator>().SetTrigger("death");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        /// <summary>
        /// Increase _currentHealth by an amount equal to restoreAmount
        /// </summary>
        /// <param name="restoreAmount"></param>
        public void Heal(float restoreAmount) {
            _currentHealth.Value = MathF.Min(MaxHealth, _currentHealth.Value + restoreAmount);
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

        #region ISaveable interface implements
        public object CaptureState() {
            return _currentHealth.Value;
        }

        public void RestoreState(object state) {
            _currentHealth.Value = (float)state;

            if (_currentHealth.Value <= 0f) {
                Die();
            }
        }
        #endregion
    }
}
