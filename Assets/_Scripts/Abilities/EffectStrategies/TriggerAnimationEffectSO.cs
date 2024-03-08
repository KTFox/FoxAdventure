using System;
using UnityEngine;

namespace RPG.Abilities.EffectStrategies
{
    [CreateAssetMenu(menuName = "ScriptableObject/EffectStrategySO/TriggerAnimationEffectSO")]
    public class TriggerAnimationEffectSO : EffectStrategySO
    {
        [SerializeField]
        private string animationParameter;

        public override void StartEffect(AbilityData data, Action finishEffect)
        {
            data.User.GetComponent<Animator>().SetTrigger(animationParameter);
            finishEffect();
        }
    }
}
