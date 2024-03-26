using System;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable
    {
        // Variables

        private List<QuestStatus> _questStatuses = new List<QuestStatus>();

        // Properties

        public IEnumerable<QuestStatus> QuestStatuses => _questStatuses;

        // Events

        public event Action OnQuestListUpdated;


        // Methods

        public void AddQuest(QuestSO newQuest)
        {
            if (HasQuest(newQuest)) return;

            QuestStatus newQuestStatus = new QuestStatus(newQuest);
            _questStatuses.Add(newQuestStatus);

            OnQuestListUpdated?.Invoke();
        }

        public void CompleteObjective(QuestSO questSO, string objectiveToComplete)
        {
            QuestStatus questStatus = GetQuestStatusOf(questSO);
            questStatus.CompleteObjective(objectiveToComplete);

            OnQuestListUpdated?.Invoke();
        }

        private bool HasQuest(QuestSO quest)
        {
            return GetQuestStatusOf(quest) != null;
        }

        private QuestStatus GetQuestStatusOf(QuestSO quest)
        {
            foreach (QuestStatus questStatus in _questStatuses)
            {
                if (questStatus.QuestSO == quest)
                {
                    return questStatus;
                }
            }

            return null;
        }

        #region ISaveable implements
        object ISaveable.CaptureState()
        {
            var state = new List<object>();

            foreach (QuestStatus questStatus in _questStatuses)
            {
                state.Add(questStatus.CaptureState());
            }

            return state;
        }

        void ISaveable.RestoreState(object state)
        {
            var stateToRestore = (List<object>)state;
            if (stateToRestore == null) return;

            _questStatuses.Clear();

            foreach (object stateObject in stateToRestore)
            {
                _questStatuses.Add(new QuestStatus(stateObject));
            }
        }
        #endregion
    }
}
