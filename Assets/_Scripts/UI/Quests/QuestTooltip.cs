using TMPro;
using UnityEngine;
using RPG.Quests;

namespace RPG.UI.Quests
{
    public class QuestTooltip : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private TextMeshProUGUI _title;
        [SerializeField]
        private Transform _objectiveContainer;
        [SerializeField]
        private GameObject _objectiveCompletedPrefab;
        [SerializeField]
        private GameObject _objectiveInCompletedPrefab;


        // Methods

        public void SetUp(QuestStatus questStatus)
        {
            _title.text = questStatus.QuestSO.QuestName;

            foreach (Transform child in _objectiveContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (QuestSO.Objective objective in questStatus.QuestSO.Objectives)
            {
                GameObject objectToSpawn;
                if (questStatus.IsCompletedObjective(objective.Reference))
                {
                    objectToSpawn = _objectiveCompletedPrefab;
                }
                else
                {
                    objectToSpawn = _objectiveInCompletedPrefab;
                }

                GameObject objectiveInstance = Instantiate(objectToSpawn, _objectiveContainer);
                TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();

                objectiveText.text = objective.Description;
            }
        }
    }
}
