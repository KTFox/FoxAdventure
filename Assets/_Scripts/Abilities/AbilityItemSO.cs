using UnityEngine;
using RPG.Inventories;

namespace RPG.Abilities
{
    [CreateAssetMenu(menuName = "ScriptableObject/Item/AbilityItemSO")]
    public class AbilityItemSO : ActionItemSO
    {
        [SerializeField]
        private TargetingStrategySO targetingStrategy;

        public override void Use(GameObject user)
        {
            targetingStrategy.StartTargeting(user);
        }
    }
}
