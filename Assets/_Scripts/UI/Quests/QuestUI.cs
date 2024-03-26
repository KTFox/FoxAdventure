using UnityEngine;
using RPG.Quests;

namespace RPG.UI.Quests
{
    public class QuestUI : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private QuestRowUI _questRowPrefab;

        private QuestList _playerQuestList;


        // Methods

        private void Start()
        {
            _playerQuestList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();

            _playerQuestList.OnQuestListUpdated += UpdateUI;

            UpdateUI();
        }

        private void UpdateUI()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            foreach (QuestStatus questStatus in _playerQuestList.QuestStatuses)
            {
                QuestRowUI questRowInstance = Instantiate(_questRowPrefab, transform);
                questRowInstance.SetUp(questStatus);
            }
        }
    }
}
