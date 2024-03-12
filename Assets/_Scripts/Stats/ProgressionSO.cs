using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(menuName = "ScriptableObject/ProgressionSO")]
    public class ProgressionSO : ScriptableObject
    {
        [SerializeField]
        private CharacterProgress[] characterProgresses;

        #region Serializable structs
        [System.Serializable]
        struct CharacterProgress
        {
            public CharacterClass characterClass;
            public StatProgress[] statProgresses;
        }

        [System.Serializable]
        struct StatProgress
        {
            public Stat stat;
            public float[] levels;
        }
        #endregion

        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable;

        /// <summary>
        /// Min traitValue of level is 1
        /// </summary>
        /// <param name="characterClass"></param>
        /// <param name="stat"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public float GetStat(CharacterClass characterClass, Stat stat, int level)
        {
            BuildLookupTable();

            float[] levels = lookupTable[characterClass][stat];

            if (levels.Length == 0)
                return 0;

            if (levels.Length < level)
                return levels[levels.Length - 1];

            return levels[level - 1];
        }

        /// <summary>
        /// Return CharacterClass.Stat.Level.Length
        /// </summary>
        /// <param name="characterClass"></param>
        /// <param name="stat"></param>
        /// <returns></returns>
        public int GetLevelLength(CharacterClass characterClass, Stat stat)
        {
            BuildLookupTable();

            float[] levels = lookupTable[characterClass][stat];

            return levels.Length;
        }

        /// <summary>
        /// Build a lookup table for performance purpose
        /// </summary>
        private void BuildLookupTable()
        {
            if (lookupTable != null) 
                return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (CharacterProgress characterProgress in characterProgresses)
            {
                Dictionary<Stat, float[]> statLookupTable = new Dictionary<Stat, float[]>();

                foreach (StatProgress statProgress in characterProgress.statProgresses)
                {
                    statLookupTable[statProgress.stat] = statProgress.levels;
                }

                lookupTable[characterProgress.characterClass] = statLookupTable;
            }
        }
    }
}
