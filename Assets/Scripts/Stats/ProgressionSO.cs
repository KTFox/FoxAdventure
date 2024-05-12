using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class ProgressionSO : ScriptableObject
    {
        // Structs

        [System.Serializable]
        private struct CharacterProgress
        {
            public CharacterClass characterClass;
            public StatProgress[] statProgresses;
        }

        [System.Serializable]
        private struct StatProgress
        {
            public Stat stat;
            public float[] levels;
        }

        // Variables

        [SerializeField]
        private CharacterProgress[] _characterProgresses;
        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> _lookupTable;


        // Methods

        public float GetStat(CharacterClass characterClass, Stat stat, int level)
        {
            BuildLookupTable();

            if (!_lookupTable[characterClass].ContainsKey(stat))
            {
                return 0;
            }

            float[] levels = _lookupTable[characterClass][stat];

            if (levels.Length == 0)
            {
                return 0;
            }

            if (levels.Length < level)
            {
                return levels[levels.Length - 1];
            }

            return levels[level - 1];
        }

        public int GetLevelLength(CharacterClass characterClass, Stat stat)
        {
            BuildLookupTable();

            float[] levels = _lookupTable[characterClass][stat];

            return levels.Length;
        }

        private void BuildLookupTable()
        {
            if (_lookupTable != null) return;

            _lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (CharacterProgress characterProgress in _characterProgresses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();

                foreach (StatProgress statProgress in characterProgress.statProgresses)
                {
                    statLookupTable[statProgress.stat] = statProgress.levels;
                }

                _lookupTable[characterProgress.characterClass] = statLookupTable;
            }
        }
    }
}
