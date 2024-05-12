using System;
using UnityEngine;

namespace RPG.Abilities
{
    public abstract class EffectStrategySO : ScriptableObject
    {
        public abstract void StartEffect(AbilityData abilityData, Action finishedCallback);
    }
}
