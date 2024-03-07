using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.FilterStrategies
{
    [CreateAssetMenu(menuName = "ScriptableObject/FilterStrategySO/TagFilterSO")]
    public class TagFilterSO : FilterStrategySO
    {
        [SerializeField]
        private string filterTag;

        public override IEnumerable<GameObject> Filter(IEnumerable<GameObject> objectsToFilter)
        {
            foreach (var gameObject in objectsToFilter)
            {
                if (gameObject.CompareTag(filterTag))
                {
                    yield return gameObject;
                }
            }
        }
    }
}
