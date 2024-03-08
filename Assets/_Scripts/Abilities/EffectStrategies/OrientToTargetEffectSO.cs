using System;
using UnityEngine;

namespace RPG.Abilities.EffectStrategies
{
    [CreateAssetMenu(menuName = "ScriptableObject/EffectStrategySO/OrientToTargetEffectSO")]
    public class OrientToTargetEffectSO : EffectStrategySO
    {
        public override void StartEffect(AbilityData data, Action finishEffect)
        {
            data.User.transform.LookAt(data.TargetPoint);
            finishEffect();
        }
    }
}
