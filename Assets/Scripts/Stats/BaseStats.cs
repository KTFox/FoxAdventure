using System;
using UnityEngine;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour {

        public event Action OnLevelUp;

        [Range(1, 3)]
        [SerializeField]
        private int startLevel = 1;
        [SerializeField]
        private CharacterClass characterClass;
        [SerializeField]
        private ProgressionSO progressionSO;
        [SerializeField]
        private GameObject levelupParticleEffect;
        [SerializeField]
        [Tooltip("Should be ticked in case this gameObject is Player")]
        private bool shouldUseModifier;

        private int currentLevel;

        private void Start() {
            currentLevel = CalculateLevel();

            Experience experience = GetComponent<Experience>();
            if (experience != null) {
                experience.OnExperienceGained += UpdateExperience;
            }
        }

        private void UpdateExperience() {
            if (currentLevel < CalculateLevel()) {
                currentLevel = CalculateLevel();
                LevelUpEffect();
                OnLevelUp?.Invoke();
            }
        }

        private int CalculateLevel() {
            //Return 1 if this gameObject doesn't have Experience component
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startLevel;

            float currentXP = experience.GetExperiencePoint();
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
            return progressionSO.GetStat(characterClass, stat, GetCurrentLevel());
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

        public int GetCurrentLevel() {
            if (currentLevel < 1) {
                currentLevel = CalculateLevel();
            }

            return currentLevel;
        }
    }
}
