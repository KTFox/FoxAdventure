using System;
using UnityEngine;

namespace RPG.Abilities.TargetingStrategies
{
    [CreateAssetMenu(menuName = "ScriptableObject/Strategy/TargetingStrategy/SelfTargeting")]
    public class SelfTargetingSO : TargetingStrategySO
    {
        public override void StartTargeting(AbilityData abilityData, Action finishedCallback)
        {
            abilityData.Targets = new GameObject[] { abilityData.Instigator };
            abilityData.TargetPoint = abilityData.Instigator.transform.position;
            finishedCallback();
        }
    }
}
