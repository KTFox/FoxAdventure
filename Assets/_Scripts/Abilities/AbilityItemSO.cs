using UnityEngine;
using RPG.Inventories;
using RPG.Attributes;

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
        [SerializeField]
        private float manaCost;

        public override void Use(GameObject user)
        {
            // Checking ability mana cost
            Mana playerMana = user.GetComponent<Mana>();
            if (playerMana.CurrentMana < manaCost) return;

            // Checking ablity cooldown time
            CooldownStore userCooldownStore = user.GetComponent<CooldownStore>();
            if (userCooldownStore.GetRemainingTIme(this) > 0) return;

            // TargetingStrategies action
            AbilityData data = new AbilityData(user);
            targetingStrategy.StartTargeting(data, () =>
            {
                GetAcquiredTargets(data);
            });
        }

        private void GetAcquiredTargets(AbilityData data)
        {
            Mana playerMana = data.User.GetComponent<Mana>();
            if (!playerMana.UseMana(manaCost)) return;

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