using UnityEngine;
using System.Collections.Generic;
using RPG.Stats;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = "ScriptableObject/InventoryItem/StatsItemSO")]
    public class StatsItemSO : EquipableItemSO, IModifierProvider
    {
        // Struct

        [System.Serializable]
        private struct Modifier
        {
            public Stat stat;
            public float value;
        }

        // Variables

        [SerializeField]
        private Modifier[] _additiveModifiers;
        [SerializeField]
        private Modifier[] _percentageModifiers;

        #region IModifierProvider implements
        IEnumerable<float> IModifierProvider.GetAdditiveModifiers(Stat stat)
        {
            foreach (var modifier in _additiveModifiers)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }

        IEnumerable<float> IModifierProvider.GetPercentageModifiers(Stat stat)
        {
            foreach (var modifier in _percentageModifiers)
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
