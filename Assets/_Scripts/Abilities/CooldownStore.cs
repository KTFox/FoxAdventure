using System.Collections.Generic;
using UnityEngine;
using RPG.Inventories;

namespace RPG.Abilities
{
    public class CooldownStore : MonoBehaviour
    {
        private Dictionary<InventoryItemSO, float> cooldownTimers = new Dictionary<InventoryItemSO, float>();
        private Dictionary<InventoryItemSO, float> initialCooldown = new Dictionary<InventoryItemSO, float>();

        private void Update()
        {
            var keys = new List<InventoryItemSO>(cooldownTimers.Keys);
            foreach (var ability in keys)
            {
                cooldownTimers[ability] -= Time.deltaTime;

                if (cooldownTimers[ability] <= 0)
                {
                    cooldownTimers.Remove(ability);
                    initialCooldown.Remove(ability);
                }
            }
        }

        public void StartCooldown(InventoryItemSO ability, float cooldownTime)
        {
            cooldownTimers[ability] = cooldownTime;
            initialCooldown[ability] = cooldownTime;
        }

        public float GetRemainingTIme(InventoryItemSO ability)
        {
            if (!cooldownTimers.ContainsKey(ability))
            {
                return 0;
            }

            return cooldownTimers[ability];
        }

        public float GetFractionTime(InventoryItemSO ability)
        {
            if (ability == null)
            {
                return 0;
            }

            if (!cooldownTimers.ContainsKey(ability))
            {
                return 0;
            }

            return cooldownTimers[ability] / initialCooldown[ability];
        }
    }
}
