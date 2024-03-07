using UnityEngine;
using System.Collections.Generic;
using RPG.Inventories;

namespace RPG.Abilities
{
    [CreateAssetMenu(menuName = "ScriptableObject/Item/AbilityItemSO")]
    public class AbilityItemSO : ActionItemSO
    {
        [SerializeField]
        private TargetingStrategySO targetingStrategy;
        [SerializeField]
        private FilterStrategySO[] filterStrategies;

        public override void Use(GameObject user)
        {
            // TargetingStrategies action
            targetingStrategy.StartTargeting(user, GetAcquiredTargets);
        }

        private void GetAcquiredTargets(IEnumerable<GameObject> acquiredTargets)
        {
            // Filter targets
            foreach (FilterStrategySO filter in filterStrategies)
            {
                acquiredTargets = filter.Filter(acquiredTargets);
            }

            foreach (GameObject target in acquiredTargets)
            {
                Debug.Log($"TargetingStrategies {target.name}");
            }
        }
    }
}
