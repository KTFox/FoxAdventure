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


        // Methods

        public void SetUp(QuestSO quest)
        {
            _title.text = quest.Title;
            _progress.text = $"0/{quest.ObjectiveCount}";
        }
    }
}
