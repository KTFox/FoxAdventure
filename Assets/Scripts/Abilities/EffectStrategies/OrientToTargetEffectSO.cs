using System;
using UnityEngine;

namespace RPG.Abilities.EffectStrategies
{
    [CreateAssetMenu(menuName = "ScriptableObject/Strategy/EffectStrategy/OrientToTargetEffect")]
    public class OrientToTargetEffectSO : EffectStrategySO
    {
        public override void StartEffect(AbilityData abilityData, Action finishedCallback)
        {
            abilityData.Instigator.transform.LookAt(abilityData.TargetPoint);
            finishedCallback();
        }
    }
}
