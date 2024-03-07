using System;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Abilities.EffectStrategies
{
    [CreateAssetMenu(menuName = "ScriptableObject/EffectStrategySO/HealthEffectSO")]
    public class HealthEffectSO : EffectStrategySO
    {
        [SerializeField]
        private float healthChange;

        public override void StartEffect(AbilityData data, Action finishEffect)
        {
            foreach (var target in data.Targets)
            {
                Health health = target.GetComponent<Health>();
                if (health)
                {
                    if (healthChange < 0)
                    {
                        health.TakeDamage(data.User, -healthChange);
                    }
                    else
                    {
                        health.Heal(healthChange);
                    }
                }
            }

            finishEffect();
        }
    }
}
