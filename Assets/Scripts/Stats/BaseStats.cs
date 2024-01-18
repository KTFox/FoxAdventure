using UnityEngine;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour {

        [Range(1, 3)]
        [SerializeField]
        private int startLevel = 1;
        [SerializeField]
        private CharacterClass characterClass;
        [SerializeField]
        private ProgressionSO progressionSO;

        public float GetStat(Stat stat) {
            return progressionSO.GetStat(characterClass, stat, startLevel);
        }
    }
}
