using System;
using UnityEngine;

namespace RPG.Abilities.TargetingStrategies
{
    [CreateAssetMenu(menuName = "ScriptableObject/TargetingStrategySO/SelfTargetingSO")]
    public class SelfTargetingSO : TargetingStrategySO
    {
        public override void StartTargeting(AbilityData data, Action finishTargeting)
        {
            data.SetTargets(new GameObject[] { data.User });
            data.SetTargetedPoint(data.User.transform.position);
            finishTargeting();
        }
    }
}
