using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestStatus
    {
        // Constructors

        public QuestStatus(QuestSO newQuest)
        {
            _questSO = newQuest;
        }

        // Variables

        private QuestSO _questSO;
        private List<string> _completedObjectives = new List<string>();

        // Properties

        public QuestSO QuestSO => _questSO;
        public List<string> CompletedObjectives => _completedObjectives;
        public int CompletedObjectivesCount => _completedObjectives.Count;


        // Methods

        public void CompleteObjective(string objectiveToComplete)
        {
            if (_questSO.HasObjective(objectiveToComplete))
            {
                _completedObjectives.Add(objectiveToComplete);
            }
        }

        public bool IsCompletedObjective(string objective)
        {
            return _completedObjectives.Contains(objective);
        }
    }
}
