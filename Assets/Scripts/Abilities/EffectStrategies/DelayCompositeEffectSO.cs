using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.EffectStrategies
{
    [CreateAssetMenu(menuName = "ScriptableObject/Strategy/EffectStrategy/DelayCompositeEffect")]
    public class DelayCompositeEffectSO : EffectStrategySO
    {
        // Variables

        [SerializeField]
        private EffectStrategySO[] _delayedEffects;
        [SerializeField]
        private float _delayTime;
        [SerializeField]
        private bool _canBeCancelled;


        // Methods

        public override void StartEffect(AbilityData abilityData, Action finishedCallback)
        {
            abilityData.StartCoroutine(DelayEffectsCoroutine(abilityData, finishedCallback));
        }

        private IEnumerator DelayEffectsCoroutine(AbilityData abilityData, Action finishedCallback)
        {
            yield return new WaitForSeconds(_delayTime);

            if (_canBeCancelled && abilityData.IsCancelled)
            {
                yield break;
            }

            foreach (var effect in _delayedEffects)
            {
                effect.StartEffect(abilityData, finishedCallback);
            }
        }
    }
}
