using UnityEngine;

namespace RPG.Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private QuestSO _questSO;
        [SerializeField]
        private string _objective;


        // Methods

        public void CompleteObjective()
        {
            QuestList playerQuestList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            playerQuestList.CompleteObjective(_questSO, _objective);
        }
    }
}
