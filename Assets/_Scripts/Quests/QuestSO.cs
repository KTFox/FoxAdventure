using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(menuName = "ScriptableObject/Quest")]
    public class QuestSO : ScriptableObject
    {
        // Variables

        [SerializeField]
        private List<string> _objectives;

        // Properties

        public string Title => name;
        public IEnumerable<string> Objectives => _objectives;
        public int ObjectiveCount => _objectives.Count;


        // Methods

        public bool HasObjective(string objectiveToCheck)
        {
            return _objectives.Contains(objectiveToCheck);
        }
    }
}
