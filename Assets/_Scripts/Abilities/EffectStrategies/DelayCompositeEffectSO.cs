using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.EffectStrategies
{
    [CreateAssetMenu(menuName = "ScriptableObject/EffectStrategySO/DelayCompositeEffectSO")]
    public class DelayCompositeEffectSO : EffectStrategySO
    {
        [SerializeField]
        private EffectStrategySO[] delayedEffects;
        [SerializeField]
        private float delayTime;
        [SerializeField]
        private bool abortIfCancelled;

        public override void StartEffect(AbilityData data, Action finishEffect)
        {
            data.StartCoroutine(DelayEffects(data, finishEffect));
        }

        private IEnumerator DelayEffects(AbilityData data, Action finishEffect)
        {
            yield return new WaitForSeconds(delayTime);

            if (abortIfCancelled && data.Cancelled) yield break;

            foreach (var effect in delayedEffects)
            {
                effect.StartEffect(data, finishEffect);
            }
        }
    }
}
