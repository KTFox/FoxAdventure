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
        [SerializeField]
        private float cooldownTime;

        public override void Use(GameObject user)
        {
            // Checking ablity cooldown time
            CooldownStore userCooldownStore = user.GetComponent<CooldownStore>();
            if (userCooldownStore.GetRemainingTIme(this) > 0)
            {
                return;
            }

            // TargetingStrategies action
            AbilityData data = new AbilityData(user);
            targetingStrategy.StartTargeting(data, () =>
            {
                GetAcquiredTargets(data);
            });
        }

        private void GetAcquiredTargets(AbilityData data)
        {
            // Set cooldown time for this ability
            CooldownStore userCooldownStore = data.User.GetComponent<CooldownStore>();
            userCooldownStore.StartCooldown(this, cooldownTime);

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
