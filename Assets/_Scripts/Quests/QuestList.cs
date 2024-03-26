using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour
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

        private bool HasQuest(QuestSO quest)
        {
            foreach (QuestStatus questStatus in _questStatuses)
            {
                if (questStatus.QuestSO == quest)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
