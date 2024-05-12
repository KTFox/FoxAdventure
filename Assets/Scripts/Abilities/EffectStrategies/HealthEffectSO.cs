using System;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Abilities.EffectStrategies
{
    [CreateAssetMenu(menuName = "ScriptableObject/Strategy/EffectStrategy/HealthEffect")]
    public class HealthEffectSO : EffectStrategySO
    {
        [SerializeField]
        private float _healthChange;


        public override void StartEffect(AbilityData abilityData, Action finishedCallback)
        {
            foreach (var target in abilityData.Targets)
            {
                Health targetHealth = target.GetComponent<Health>();
                if (targetHealth)
                {
                    if (_healthChange < 0)
                    {
                        targetHealth.TakeDamage(abilityData.Instigator, -_healthChange);
                    }
                    else
                    {
                        targetHealth.Heal(_healthChange);
                    }
                }
            }

            finishedCallback();
        }
    }
}
