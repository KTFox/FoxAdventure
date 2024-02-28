using UnityEngine;
using System.Collections.Generic;
using RPG.Stats;

namespace RPG.Inventory
{
    [CreateAssetMenu(menuName = "ScriptableObject/Item/StatsItemSO")]
    public class StatsItemSO : EquipableItemSO, IModifierProvider
    {
        [SerializeField]
        private Modifier[] additiveModifiers;
        [SerializeField]
        private Modifier[] percentageModifiers;

        [System.Serializable]
        private struct Modifier
        {
            public Stat stat;
            public float value;
        }

        #region IModifierProvider implements
        IEnumerable<float> IModifierProvider.GetAdditiveModifiers(Stat stat)
        {
            foreach (var modifier in additiveModifiers)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }

        IEnumerable<float> IModifierProvider.GetPercentageModifiers(Stat stat)
        {
            foreach (var modifier in percentageModifiers)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }
        #endregion
    }
}
