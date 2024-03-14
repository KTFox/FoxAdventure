using System;
using UnityEngine;
using UnityEngine.Events;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using RPG.Utility;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        public UnityEvent OnDie;

        [SerializeField]
        private UnityEvent<float> OnTakeDamage;

        private LazyValue<float> _currentHealth;
        private bool wasDeadLastFrame;

        #region Properties
        public float CurrentHealth => _currentHealth.Value;
        public float CurrentHealthFraction => _currentHealth.Value / GetComponent<BaseStats>().GetStat(Stat.Health);
        public float CurrentHealthPercentage => CurrentHealthFraction * 100;
        public float MaxHealth => GetComponent<BaseStats>().GetStat(Stat.Health);
        public bool IsDead => _currentHealth.Value <= 0f;
        #endregion

        private void Awake()
        {
            _currentHealth = new LazyValue<float>(GetInitialHealth);
        }

        float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;
        }

        void RegenerateHealth()
        {
            _currentHealth.Value = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start()
        {
            _currentHealth.ForceInit();
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().OnLevelUp -= RegenerateHealth;
        }

        /// <summary>
        /// Reduce _currentHealth by an amount equal to damage.
        /// Callback OnDie() and OnTakeDamage() event.
        /// </summary>
        /// <param name="instigator"></param>
        /// <param name="damage"></param>
        public void TakeDamage(GameObject instigator, float damage)
        {
            _currentHealth.Value = Mathf.Max(_currentHealth.Value - damage, 0f);

            if (IsDead)
            {
                AwardExperience(instigator);
                OnDie?.Invoke();
            }
            else
            {
                OnTakeDamage?.Invoke(damage);
            }

            UpdateState();
        }

        /// <summary>
        /// Increase _currentHealth by an amount equal to restoreAmount
        /// </summary>
        /// <param name="restoreAmount"></param>
        public void Heal(float restoreAmount)
        {
            _currentHealth.Value = MathF.Min(MaxHealth, _currentHealth.Value + restoreAmount);
            UpdateState();
        }

        private void UpdateState()
        {
            Animator animator = GetComponent<Animator>();

            if (!wasDeadLastFrame && IsDead)
            {
                animator.SetTrigger("death");
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }

            if (wasDeadLastFrame && !IsDead)
            {
                animator.Rebind();
            }

            wasDeadLastFrame = IsDead;
        }

        /// <summary>
        /// Gain instigator's experience 
        /// </summary>
        /// <param name="instigator"></param>
        private void AwardExperience(GameObject instigator)
        {
            Experience instigatorExperience = instigator.GetComponent<Experience>();
            if (instigatorExperience == null) return;

            instigatorExperience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        #region ISaveable interface implements
        object ISaveable.CaptureState()
        {
            return _currentHealth.Value;
        }

        void ISaveable.RestoreState(object state)
        {
            _currentHealth.Value = (float)state;

            UpdateState();
        }
        #endregion
    }
}
