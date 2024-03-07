using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities
{
    public class CooldownStore : MonoBehaviour
    {
        private Dictionary<AbilityItemSO, float> cooldownTimers = new Dictionary<AbilityItemSO, float>();

        private void Update()
        {
            var keys = new List<AbilityItemSO>(cooldownTimers.Keys);
            foreach (var ability in keys)
            {
                cooldownTimers[ability] -= Time.deltaTime;

                if (cooldownTimers[ability] <= 0)
                {
                    cooldownTimers.Remove(ability);
                }
            }
        }

        public void StartCooldown(AbilityItemSO ability, float cooldownTime)
        {
            cooldownTimers[ability] = cooldownTime;
        }

        public float GetRemainingTIme(AbilityItemSO ability)
        {
            if (!cooldownTimers.ContainsKey(ability))
            {
                return 0;
            }

            return cooldownTimers[ability];
        }
    }
}
