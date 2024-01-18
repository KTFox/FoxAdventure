using UnityEngine;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "New ProgressionSO", menuName = "Stats/Create new ProgressionSO", order = 0)]
    public class ProgressionSO : ScriptableObject {

        [SerializeField]
        private CharacterProgress[] characterProgresses;

        public float GetStat(CharacterClass characterClass, Stat stat, int level) {
            foreach (CharacterProgress characterProgress in characterProgresses) {
                if (characterProgress.characterClass != characterClass) continue;

                foreach (StatProgress statProgress in characterProgress.statProgresses) {
                    if (statProgress.stat != stat) continue;
                    if (statProgress.levels.Length < level) continue;

                    return statProgress.levels[level - 1];
                }
            }

            return 0;
        }

        [System.Serializable]
        class CharacterProgress {
            public CharacterClass characterClass;
            public StatProgress[] statProgresses;
        }

        [System.Serializable]
        class StatProgress {
            public Stat stat;
            public int[] levels;
        }
    }
}
