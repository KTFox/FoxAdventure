using UnityEngine;

namespace RPG.Abilities
{
    public abstract class TargetingStrategySO : ScriptableObject
    {
        public abstract void StartTargeting(GameObject user);
    }
}
