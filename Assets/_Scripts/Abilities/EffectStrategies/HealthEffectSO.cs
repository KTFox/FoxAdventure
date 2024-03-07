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

        public override void StartEffect(GameObject user, IEnumerable<GameObject> targets, Action finishEffect)
        {
            foreach (var target in targets)
            {
                Health health = target.GetComponent<Health>();
                if (health)
                {
                    if (healthChange < 0)
                    {
                        health.TakeDamage(user, -healthChange);
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
