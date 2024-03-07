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
        [SerializeField]
        private EffectStrategySO[] effectStrategies;

        public override void Use(GameObject user)
        {
            // TargetingStrategies action
            targetingStrategy.StartTargeting(user, (IEnumerable<GameObject> targets) =>
            {
                GetAcquiredTargets(user, targets);
            });
        }

        private void GetAcquiredTargets(GameObject user, IEnumerable<GameObject> acquiredTargets)
        {
            // Filter targets
            foreach (FilterStrategySO filter in filterStrategies)
            {
                acquiredTargets = filter.Filter(acquiredTargets);
            }

            // Apply effects
            foreach (EffectStrategySO effect in effectStrategies)
            {
                effect.StartEffect(user, acquiredTargets, finishEffect);
            }
        }

        private void finishEffect()
        {

        }
    }
}
