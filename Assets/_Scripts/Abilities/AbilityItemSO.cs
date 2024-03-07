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

        public override void Use(GameObject user)
        {
            // Targeting action
            targetingStrategy.StartTargeting(user, GetAcquiredTargets);
        }

        private void GetAcquiredTargets(IEnumerable<GameObject> acquiredTargets)
        {
            foreach (GameObject target in acquiredTargets)
            {
                Debug.Log($"Targeting {target.name}");
            }
        }
    }
}
