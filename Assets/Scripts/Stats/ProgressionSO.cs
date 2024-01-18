using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "New ProgressionSO", menuName = "Stats/Create new ProgressionSO", order = 0)]
    public class ProgressionSO : ScriptableObject {

        [SerializeField]
        private CharacterProgress[] characterProgresses;

        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable;

        public float GetStat(CharacterClass characterClass, Stat stat, int level) {
            BuildLookupTable();

            float[] levels = lookupTable[characterClass][stat];

            if (levels.Length < level) {
                return 0;
            }

            return levels[level - 1];
        }

        private void BuildLookupTable() {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (CharacterProgress characterProgress in characterProgresses) {
                Dictionary<Stat, float[]> statLookupTable = new Dictionary<Stat, float[]>();

                foreach (StatProgress statProgress in characterProgress.statProgresses) {
                    statLookupTable[statProgress.stat] = statProgress.levels;
                }

                lookupTable[characterProgress.characterClass] = statLookupTable;
            }
        }

        [System.Serializable]
        class CharacterProgress {
            public CharacterClass characterClass;
            public StatProgress[] statProgresses;
        }

        [System.Serializable]
        class StatProgress {
            public Stat stat;
            public float[] levels;
        }
    }
}
