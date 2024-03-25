using UnityEngine;
using RPG.Quests;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private QuestSO[] _tempQuests;
        [SerializeField]
        private QuestRowUI _questRowPrefab;


        // Methods
        private void Start()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            foreach (QuestSO questSO in _tempQuests)
            {
                QuestRowUI questRowInstance = Instantiate(_questRowPrefab, transform);
                questRowInstance.SetUp(questSO);
            }
        }
    }
}
