using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private QuestStatus[] _questStatuses;


        // Properties

        public IEnumerable<QuestStatus> QuestStatuses => _questStatuses;
    }
}
