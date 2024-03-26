using UnityEngine;
using RPG.Quests;

namespace RPG.UI.Quests
{
    public class QuestUI : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private QuestRowUI _questRowPrefab;


        // Methods
        private void Start()
        {
            QuestList playerQuestList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            foreach (QuestStatus questStatus in playerQuestList.QuestStatuses)
            {
                QuestRowUI questRowInstance = Instantiate(_questRowPrefab, transform);
                questRowInstance.SetUp(questStatus);
            }
        }
    }
}
