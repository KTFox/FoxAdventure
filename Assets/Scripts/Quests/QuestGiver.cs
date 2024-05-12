using UnityEngine;

namespace RPG.Quests
{
    public class QuestGiver : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private QuestSO _givingQuest;


        // Methods

        public void GiveQuest()
        {
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.AddQuest(_givingQuest);
        }
    }
}
