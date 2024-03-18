using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = "ScriptableObject/DropLibrarySO")]
    public class DropLibrarySO : ScriptableObject
    {
        // Structs

        public struct Dropped
        {
            public InventoryItemSO item;
            public int number;
        }

        [System.Serializable]
        private class DropConfig
        {
            public InventoryItemSO item;

            [Tooltip("The bigger value, the more chance to be dropped")]
            public float[] relativeChances;

            public int[] minNumbers;
            public int[] maxNumbers;

            public int GetRandomNumber(int level)
            {
                if (!item.Stackable)
                    return 1;

                int minNumber = GetByLevel(minNumbers, level);
                int maxNumber = GetByLevel(maxNumbers, level);

                return Random.Range(minNumber, maxNumber + 1);
            }
        }

        // Variables

        [Tooltip("List of DropConfig that enemy can dropConfig.")]
        [SerializeField]
        private DropConfig[] _dropConfigs;
        [Tooltip("This value will determine the chance that enemy will dropConfig items.")]
        [SerializeField]
        private float[] _dropChancePercentage;
        [Tooltip("Min _quantity of items that enemy can dropConfig.")]
        [SerializeField]
        private int[] _minDropQuantity;
        [Tooltip("Max _quantity of pickups that enemy can dropConfig.")]
        [SerializeField]
        private int[] _maxDropQuantity;


        // Methods

        /// <summary>
        /// Return dropped items and their quantity.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public IEnumerable<Dropped> GetRandomDrops(int level)
        {
            if (!ShouldDrop(level))
            {
                yield break;
            }

            for (int i = 0; i < GetRandomNumberOfDrops(level); i++)
            {
                yield return GetRandomDrop(level);
            }
        }

        private bool ShouldDrop(int level)
        {
            return Random.Range(0, 100f) < GetByLevel(_dropChancePercentage, level);
        }

        private int GetRandomNumberOfDrops(int level)
        {
            int min = GetByLevel(_minDropQuantity, level);
            int max = GetByLevel(_maxDropQuantity, level);

            return Random.Range(min, max + 1);
        }

        private Dropped GetRandomDrop(int level)
        {
            var dropConfig = SelectRandomDropConfig(level);
            var drop = new Dropped();

            drop.item = dropConfig.item;
            drop.number = dropConfig.GetRandomNumber(level);

            return drop;
        }

        private DropConfig SelectRandomDropConfig(int level)
        {
            float totalRelativeChance = GetTotalRelativeChance(level);
            float randomRoll = Random.Range(0, totalRelativeChance);
            float chanceTotal = 0;

            foreach (var dropConfig in _dropConfigs)
            {
                chanceTotal += GetByLevel(dropConfig.relativeChances, level);

                if (chanceTotal > randomRoll)
                {
                    return dropConfig;
                }
            }

            return null;
        }

        private float GetTotalRelativeChance(int level)
        {
            float total = 0;

            foreach (var dropConfig in _dropConfigs)
            {
                total += GetByLevel(dropConfig.relativeChances, level);
            }

            return total;
        }

        private static T GetByLevel<T>(T[] values, int level)
        {
            if (values.Length == 0)
            {
                return default;
            }

            if (level > values.Length)
            {
                return values[values.Length - 1];
            }

            if (level < 1)
            {
                return default;
            }

            return values[level - 1];
        }
    }
}
