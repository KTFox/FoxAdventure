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
        // Variables

        private LazyValue<float> _currentHealth;
        private bool _wasDeadLastFrame;

        // Properties

        public float CurrentHealth => _currentHealth.Value;
        public float CurrentHealthFraction => _currentHealth.Value / GetComponent<BaseStats>().GetValueOfStat(Stat.Health);
        public float CurrentHealthPercentage => CurrentHealthFraction * 100;
        public float MaxHealth => GetComponent<BaseStats>().GetValueOfStat(Stat.Health);
        public bool IsDead => _currentHealth.Value <= 0f;

        // Events

        public UnityEvent OnDeath;

        [SerializeField]
        private UnityEvent<float> OnTakingDamage;


        // Methods

        private void Awake()
        {
            _currentHealth = new LazyValue<float>(GetInitialHealth);
        }

        float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetValueOfStat(Stat.Health);
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().OnLevelUp += BaseStats_LevelUp;
        }

        void BaseStats_LevelUp()
        {
            _currentHealth.Value = GetComponent<BaseStats>().GetValueOfStat(Stat.Health);
        }

        private void Start()
        {
            _currentHealth.ForceInit();
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().OnLevelUp -= BaseStats_LevelUp;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            _currentHealth.Value = Mathf.Max(_currentHealth.Value - damage, 0f);

            if (IsDead)
            {
                AwardExperience(instigator);
                OnDeath?.Invoke();
            }
            else
            {
                OnTakingDamage?.Invoke(damage);
            }

            UpdateState();
        }

        public void Heal(float healingAmount)
        {
            _currentHealth.Value = MathF.Min(MaxHealth, _currentHealth.Value + healingAmount);
            UpdateState();
        }

        private void AwardExperience(GameObject instigator)
        {
            var instigatorExperience = instigator.GetComponent<Experience>();
            if (instigatorExperience == null) return;

            instigatorExperience.GainExperience(GetComponent<BaseStats>().GetValueOfStat(Stat.ExperienceReward));
        }

        private void UpdateState()
        {
            var animator = GetComponent<Animator>();

            if (!_wasDeadLastFrame && IsDead)
            {
                animator.SetTrigger("death");
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }

            if (_wasDeadLastFrame && !IsDead)
            {
                animator.Rebind();
            }

            _wasDeadLastFrame = IsDead;
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
