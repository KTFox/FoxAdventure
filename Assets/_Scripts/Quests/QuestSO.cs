using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(menuName = "ScriptableObject/Quest")]
    public class QuestSO : ScriptableObject
    {
        // Variables

        [SerializeField]
        private string[] _objectives;

        // Properties

        public string Title => name;
        public int ObjectiveCount => _objectives.Length;
    }
}
