using System.Collections.Generic;
using UnityEngine;
using RPG.Inventories;

namespace RPG.Quests
{
    [CreateAssetMenu(menuName = "ScriptableObject/Quest")]
    public class QuestSO : ScriptableObject
    {
        // Variables

        [SerializeField]
        private List<Objective> _objectives = new List<Objective>();
        [SerializeField]
        private List<Reward> _rewards = new List<Reward>();

        // Properties

        public string QuestName => name;
        public IEnumerable<Objective> Objectives => _objectives;
        public int ObjectiveCount => _objectives.Count;
        public IEnumerable<Reward> Rewards => _rewards;

        // Structs

        [System.Serializable]
        public class Objective
        {
            public string Reference;
            public string Description;
        }

        [System.Serializable]
        public class Reward
        {
            [Min(1)]
            public int Quantity;

            public InventoryItemSO Item;
        }


        // Methods

        public bool HasObjective(string objectiveReference)
        {
            foreach (Objective objective in Objectives)
            {
                if (objective.Reference == objectiveReference)
                {
                    return true;
                }
            }

            return false;
        }

        public static QuestSO GetQuestSOByName(string questName)
        {
            foreach (QuestSO questSO in Resources.LoadAll<QuestSO>(""))
            {
                if (questSO.name == questName)
                {
                    return questSO;
                }
            }

            return null;
        }
    }
}
