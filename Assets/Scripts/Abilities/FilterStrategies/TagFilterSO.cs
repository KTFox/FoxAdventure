using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.FilterStrategies
{
    [CreateAssetMenu(menuName = "ScriptableObject/Strategy/FilterStrategy/TagFilter")]
    public class TagFilterSO : FilterStrategySO
    {
        [SerializeField]
        private string _filterTag;


        public override IEnumerable<GameObject> GetFilteredGameObjects(IEnumerable<GameObject> objectsToFilter)
        {
            foreach (var gameObject in objectsToFilter)
            {
                if (gameObject.CompareTag(_filterTag))
                {
                    yield return gameObject;
                }
            }
        }
    }
}
