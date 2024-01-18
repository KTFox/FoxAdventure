using UnityEngine;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "New ProgressionSO", menuName = "Stats/Create new ProgressionSO", order = 0)]
    public class ProgressionSO : ScriptableObject {

        [SerializeField]
        private CharacterProgress[] characterProgresses;

        public float GetHealth(CharacterClass characterClass, int level) {
            return 0;
        }

        [System.Serializable]
        class CharacterProgress {
            public CharacterClass characterClass;
            public StatProgress[] characterStats;
        }

        [System.Serializable]
        class StatProgress {
            public Stat stat;
            public int[] levels;
        }
    }
}
