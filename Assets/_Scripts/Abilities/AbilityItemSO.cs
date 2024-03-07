using UnityEngine;
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
            AbilityData data = new AbilityData(user);

            // TargetingStrategies action
            targetingStrategy.StartTargeting(data, () =>
            {
                GetAcquiredTargets(data);
            });
        }

        private void GetAcquiredTargets(AbilityData data)
        {
            // Filter targets
            foreach (FilterStrategySO filter in filterStrategies)
            {
                data.SetTargets(filter.Filter(data.Targets));
            }

            // Apply effects
            foreach (EffectStrategySO effect in effectStrategies)
            {
                effect.StartEffect(data, finishEffect);
            }
        }

        private void finishEffect()
        {

        }
    }
}
