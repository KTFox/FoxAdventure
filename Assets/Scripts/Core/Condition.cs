using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [System.Serializable]
    public class Condition
    {
        // Variables

        [SerializeField]
        private string _predicate;
        [SerializeField]
        private string[] _parameters;


        // Methods

        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach (var evaluator in evaluators)
            {
                bool? result = evaluator.Evaluate(_predicate, _parameters);
                if (result == null) continue;

                if (result == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
