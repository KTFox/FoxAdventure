using TMPro;
using UnityEngine;
using RPG.Quests;

namespace RPG.UI.Quests
{
    public class QuestRowUI : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private TextMeshProUGUI _title;
        [SerializeField]
        private TextMeshProUGUI _progress;

        private QuestStatus _questStatus;

        // Properties

        public QuestStatus QuestStatus => _questStatus;


        // Methods

        public void SetUp(QuestStatus questStatus)
        {
            _questStatus = questStatus;
            _title.text = questStatus.QuestSO.QuestName;
            _progress.text = $"{questStatus.CompletedObjectivesCount}/{questStatus.QuestSO.ObjectiveCount}";
        }
    }
}
