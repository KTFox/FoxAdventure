using System;
using UnityEngine;
using RPG.Utility;
using static Cinemachine.DocumentationSortingAttribute;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private ProgressionSO _progressionSO;
        [SerializeField]
        private CharacterClass _characterClass;

        [Range(1, 3)]
        [SerializeField]
        private int _startLevel = 1;

        [Tooltip("Should be ticked in case this gameObject is Player")]
        [SerializeField]
        private bool _shouldUseModifier;

        [SerializeField]
        private GameObject _levelupParticleEffect;

        private Experience _experience;
        private LazyValue<int> _currentLevel;

        // Properties

        public int CurrentLevel => _currentLevel.Value;
        public float ExperienceToLevelUp => _progressionSO.GetStat(_characterClass, Stat.ExperienceToLevelUp, CurrentLevel);

        // Events

        public event Action OnLevelUp;


        // Methods

        private void Awake()
        {
            _experience = GetComponent<Experience>();
            _currentLevel = new LazyValue<int>(GetCurrentLevel);
        }

        private void OnEnable()
        {
            if (_experience != null)
            {
                _experience.OnExperienceGained += Experience_ExperienceGained;
            }
        }

        private void OnDisable()
        {
            if (_experience != null)
            {
                _experience.OnExperienceGained -= Experience_ExperienceGained;
            }
        }

        void Experience_ExperienceGained()
        {
            if (_currentLevel.Value < GetCurrentLevel())
            {
                _currentLevel.Value = GetCurrentLevel();
                GenerateLevelUpEffect();
                OnLevelUp?.Invoke();
            }
        }

        public float GetValueOfStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        private float GetBaseStat(Stat stat)
        {
            return _progressionSO.GetStat(_characterClass, stat, _currentLevel.Value);
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!_shouldUseModifier)
            {
                return 0;
            }

            float total = 0;

            foreach (IModifierProvider modifierProvider in GetComponents<IModifierProvider>())
            {
                foreach (float additiveModifier in modifierProvider.GetAdditiveModifiers(stat))
                {
                    total += additiveModifier;
                }
            }

            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!_shouldUseModifier)
            {
                return 0;
            }

            float total = 0;

            foreach (IModifierProvider modifierProvider in GetComponents<IModifierProvider>())
            {
                foreach (float percentageModifier in modifierProvider.GetPercentageModifiers(stat))
                {
                    total += percentageModifier;
                }
            }

            return total;
        }

        private int GetCurrentLevel()
        {
            var experience = GetComponent<Experience>();

            if (experience == null)
            {
                return _startLevel;
            }

            float currentXP = experience.ExperiencePoint;
            int penultimateLevel = _progressionSO.GetLevelLength(_characterClass, Stat.ExperienceToLevelUp);

            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = _progressionSO.GetStat(_characterClass, Stat.ExperienceToLevelUp, level);

                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }

        private void GenerateLevelUpEffect()
        {
            Instantiate(_levelupParticleEffect, transform);
        }
    }
}
