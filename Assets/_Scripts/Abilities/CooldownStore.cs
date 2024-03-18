using System.Collections.Generic;
using UnityEngine;
using RPG.Inventories;

namespace RPG.Abilities
{
    public class CooldownStore : MonoBehaviour
    {
        // Variables

        private Dictionary<InventoryItemSO, float> _cooldownTimers = new Dictionary<InventoryItemSO, float>();
        private Dictionary<InventoryItemSO, float> _initialCooldowns = new Dictionary<InventoryItemSO, float>();


        // Methods

        private void Update()
        {
            OperateCooldownTimers();
        }

        void OperateCooldownTimers()
        {
            var keys = new List<InventoryItemSO>(_cooldownTimers.Keys);

            foreach (var ability in keys)
            {
                _cooldownTimers[ability] -= Time.deltaTime;

                if (_cooldownTimers[ability] <= 0)
                {
                    _cooldownTimers.Remove(ability);
                    _initialCooldowns.Remove(ability);
                }
            }
        }

        public void StartCooldown(InventoryItemSO ability, float cooldownTime)
        {
            _cooldownTimers[ability] = cooldownTime;
            _initialCooldowns[ability] = cooldownTime;
        }

        public float GetRemainingTime(InventoryItemSO ability)
        {
            if (!_cooldownTimers.ContainsKey(ability))
            {
                return 0;
            }

            return _cooldownTimers[ability];
        }

        public float GetFractionTime(InventoryItemSO ability)
        {
            if (ability == null || !_cooldownTimers.ContainsKey(ability))
            {
                return 0;
            }

            return _cooldownTimers[ability] / _initialCooldowns[ability];
        }
    }
}
