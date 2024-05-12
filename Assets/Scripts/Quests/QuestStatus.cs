using System.Collections.Generic;

namespace RPG.Quests
{
    public class QuestStatus
    {
        // Variables

        private QuestSO _questSO;
        private List<string> _completedObjectiveReferences = new List<string>();

        // Properties

        public QuestSO QuestSO => _questSO;
        public List<string> CompletedObjectiveReferences => _completedObjectiveReferences;
        public int CompletedObjectivesCount => _completedObjectiveReferences.Count;

        // Constructors

        public QuestStatus(QuestSO newQuest)
        {
            _questSO = newQuest;
        }

        public QuestStatus(object stateObject)
        {
            QuestStatusRecord questStatusRecord = stateObject as QuestStatusRecord;

            _questSO = QuestSO.GetQuestSOByName(questStatusRecord.Name);
            _completedObjectiveReferences = questStatusRecord.CompletedObjectives;
        }

        // Structs

        [System.Serializable]
        private class QuestStatusRecord
        {
            public string Name;
            public List<string> CompletedObjectives;
        }


        // Methods

        public object CaptureState()
        {
            QuestStatusRecord questStatusRecord = new QuestStatusRecord();
            questStatusRecord.Name = _questSO.QuestName;
            questStatusRecord.CompletedObjectives = _completedObjectiveReferences;

            return questStatusRecord;
        }

        public void CompleteObjective(string objectiveToComplete)
        {
            if (_questSO.HasObjective(objectiveToComplete))
            {
                _completedObjectiveReferences.Add(objectiveToComplete);
            }
        }

        public bool IsCompletedObjective(string objective)
        {
            return _completedObjectiveReferences.Contains(objective);
        }

        public bool HasCompleted()
        {
            foreach (var objective in _questSO.Objectives)
            {
                if (!_completedObjectiveReferences.Contains(objective.Reference))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
