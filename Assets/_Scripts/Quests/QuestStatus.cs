using System.Collections.Generic;

namespace RPG.Quests
{
    public class QuestStatus
    {
        // Variables

        private QuestSO _questSO;
        private List<string> _completedObjectives = new List<string>();

        // Properties

        public QuestSO QuestSO => _questSO;
        public List<string> CompletedObjectives => _completedObjectives;
        public int CompletedObjectivesCount => _completedObjectives.Count;

        // Constructors

        public QuestStatus(QuestSO newQuest)
        {
            _questSO = newQuest;
        }

        public QuestStatus(object stateObject)
        {
            QuestStatusRecord questStatusRecord = stateObject as QuestStatusRecord;

            _questSO = QuestSO.GetQuestSOByName(questStatusRecord.Name);
            _completedObjectives = questStatusRecord.CompletedObjectives;
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
            questStatusRecord.CompletedObjectives = _completedObjectives;

            return questStatusRecord;
        }

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
