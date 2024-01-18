using UnityEngine;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour {

        [Range(1,3)]
        [SerializeField]
        private int startLevel = 1;
        [SerializeField]
        private CharacterClass characterClass;
        [SerializeField]
        private ProgressionSO progressionSO;

        public float GetHealth() {
            return progressionSO.GetHealth(characterClass, startLevel);
        }

        public float GetExperienceReward() {
            return 10;
        }
    }
}
