using UnityEngine;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour {

        [Range(1,99)]
        [SerializeField]
        private int startLevel;

        [SerializeField]
        private CharacterClass characterClass;
    }
}
