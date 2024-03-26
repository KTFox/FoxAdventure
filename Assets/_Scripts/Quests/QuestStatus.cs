using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [System.Serializable]
    public class QuestStatus
    {
        // Variables

        [SerializeField]
        private QuestSO _questSO;
        [SerializeField]
        private List<string> _completedObjectives;

        // Properties

        public QuestSO QuestSO => _questSO;
        public List<string> CompletedObjectives => _completedObjectives;
        public int CompletedObjectivesCount => _completedObjectives.Count;


        // Methods

        public bool IsCompletedObjective(string objective)
        {
            return _completedObjectives.Contains(objective);
        }
    }
}
