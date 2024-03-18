using System;
using UnityEngine;

namespace RPG.Abilities.EffectStrategies
{
    [CreateAssetMenu(menuName = "ScriptableObject/Strategy/EffectStrategy/TriggerAnimationEffect")]
    public class TriggerAnimationEffectSO : EffectStrategySO
    {
        [SerializeField]
        private string _triggerParameter;


        public override void StartEffect(AbilityData abilityData, Action finishedCallback)
        {
            abilityData.Instigator.GetComponent<Animator>().SetTrigger(_triggerParameter);
            finishedCallback();
        }
    }
}
