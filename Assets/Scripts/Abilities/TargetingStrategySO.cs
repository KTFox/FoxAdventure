using System;
using UnityEngine;

namespace RPG.Abilities
{
    public abstract class TargetingStrategySO : ScriptableObject
    {
        public abstract void StartTargeting(AbilityData abilityData, Action finishedCallback);
    }
}
