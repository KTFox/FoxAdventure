using UnityEngine;
using RPG.Inventories;
using RPG.Attributes;
using RPG.Core;

namespace RPG.Abilities
{
    [CreateAssetMenu(menuName = "ScriptableObject/InventoryItem/AbilityItem")]
    public class AbilityItemSO : ActionItemSO
    {
        // Variables

        [SerializeField]
        private TargetingStrategySO _targetingStrategy;
        [SerializeField]
        private FilterStrategySO[] _filterStrategies;
        [SerializeField]
        private EffectStrategySO[] _effectStrategies;
        [SerializeField]
        private float _cooldownTime;
        [SerializeField]
        private float _manaCost;


        // Methods

        public override bool UseActionItem(GameObject instigator)
        {
            // Checking if current mana is enough for using ability
            var instigatorMana = instigator.GetComponent<Mana>();
            if (instigatorMana.CurrentMana < _manaCost)
            {
                return false;
            }

            // Checking ability cooldown time
            var instigatorCooldownStore = instigator.GetComponent<CooldownStore>();
            if (instigatorCooldownStore.GetRemainingTime(this) > 0)
            {
                return false;
            }

            // Initiate AbilityData
            var abilityData = new AbilityData(instigator);

            // Call StartAction method from ActionScheduler
            var actionScheduler = instigator.GetComponent<ActionScheduler>();
            actionScheduler.StartAction(abilityData);

            // Start Targeting strategy
            _targetingStrategy.StartTargeting(abilityData, () =>
            {
                ApplyEffectForFilteredTargets(abilityData);
            });

            return true;
        }

        private void ApplyEffectForFilteredTargets(AbilityData abilityData)
        {
            // Return if Ability is cancelled while in _targetingStrategy state
            if (abilityData.IsCancelled)
            {
                return;
            }

            // Check if the _instigator's mana is enough for using ability
            var instigatorMana = abilityData.Instigator.GetComponent<Mana>();
            if (!instigatorMana.UseMana(_manaCost))
            {
                return;
            }

            // Set cooldown time for this ability
            var instigatorCooldownStore = abilityData.Instigator.GetComponent<CooldownStore>();
            instigatorCooldownStore.StartCooldown(this, _cooldownTime);

            // Set filterd targets to abilityData's targets
            foreach (FilterStrategySO filter in _filterStrategies)
            {
                abilityData.Targets = filter.GetFilteredGameObjects(abilityData.Targets);
            }

            // Start effect strategies
            foreach (EffectStrategySO effect in _effectStrategies)
            {
                effect.StartEffect(abilityData, finishedCallback);
            }
        }

        private void finishedCallback() { }
    }
}
