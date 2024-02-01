using System;
using UnityEngine;
using RPG.Utility;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour {

        public event Action OnLevelUp;

        [SerializeField] 
        private ProgressionSO progressionSO;
        [SerializeField] 
        private CharacterClass characterClass;

        [Range(1, 3)]
        [SerializeField] 
        private int startLevel = 1;

        [Tooltip("Should be ticked in case this gameObject is Player")]
        [SerializeField] 
        private bool shouldUseModifier;

        [SerializeField] 
        private GameObject levelupParticleEffect;

        private Experience experience;
        private LazyValue<int> currentLevel;

        public int CurrentLevel {
            get {
                return currentLevel.Value;
            }
        }

        private void Awake() {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void OnEnable() {
            if (experience != null) {
                experience.OnExperienceGained += UpdateExperience;
            }
        }

        private void Start() {
            currentLevel.ForceInit();
        }

        private void OnDisable() {
            if (experience != null) {
                experience.OnExperienceGained -= UpdateExperience;
            }
        }

        private void UpdateExperience() {
            if (currentLevel.Value < CalculateLevel()) {
                currentLevel.Value = CalculateLevel();
                LevelUpEffect();
                OnLevelUp?.Invoke();
            }
        }

        private int CalculateLevel() {
            Experience experience = GetComponent<Experience>();

            //Return 1 if this gameObject doesn't have Experience component
            if (experience == null) return startLevel;

            float currentXP = experience.ExperiencePoint;
            int penultimateLevel = progressionSO.GetLevelLength(characterClass, Stat.ExperienceToLevelUp);
            for (int level = 1; level <= penultimateLevel; level++) {
                float XPToLevelUp = progressionSO.GetStat(characterClass, Stat.ExperienceToLevelUp, level);
                if (XPToLevelUp > currentXP) {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }

        private void LevelUpEffect() {
            Instantiate(levelupParticleEffect, transform);
        }

        public float GetStat(Stat stat) {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        private float GetBaseStat(Stat stat) {
            return progressionSO.GetStat(characterClass, stat, currentLevel.Value);
        }

        private float GetAdditiveModifier(Stat stat) {
            if (!shouldUseModifier) return 0;

            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>()) {
                foreach (float modifier in provider.GetAdditiveModifiers(stat)) {
                    total += modifier;
                }
            }

            return total;
        }

        private float GetPercentageModifier(Stat stat) {
            if (!shouldUseModifier) return 0;

            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>()) {
                foreach (float modifier in provider.GetPercentageModifiers(stat)) {
                    total += modifier;
                }
            }

            return total;
        }
    }
}
