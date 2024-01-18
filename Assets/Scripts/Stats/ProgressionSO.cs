using UnityEngine;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "New ProgressionSO", menuName = "Stats/Create new ProgressionSO", order = 0)]
    public class ProgressionSO : ScriptableObject {

        [SerializeField]
        private ProgressionCharacterClass[] progressionCharacterclasses;

        public float GetHealth(CharacterClass characterClass, int level) {
            foreach (ProgressionCharacterClass progressionCharacterClass in progressionCharacterclasses) {
                if (progressionCharacterClass.characterClass == characterClass) {
                    return progressionCharacterClass.healths[level - 1];
                }
            }

            return 0;
        }

        [System.Serializable]
        class ProgressionCharacterClass {
            public CharacterClass characterClass;
            public float[] healths;
        }
    }
}
