using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = "ScriptableObject/DropLibrarySO")]
    public class DropLibrarySO : ScriptableObject
    {
        [Tooltip("List of DropConfig that enemy can drop.")]
        [SerializeField]
        private DropConfig[] potentialDrops;

        [System.Serializable]
        private class DropConfig
        {
            public InventoryItemSO item;

            [Tooltip("The bigger traitValue, the more chance to be dropped")]
            public float[] relativeChances;

            public int[] minNumbers;
            public int[] maxNumbers;

            /// <summary>
            /// Get random number of items to be dropped.
            /// </summary>
            /// <param name="level"></param>
            /// <returns></returns>
            public int GetRandomNumber(int level)
            {
                if (!item.Stackable)
                    return 1;

                int minNumber = GetByLevel(minNumbers, level);
                int maxNumber = GetByLevel(maxNumbers, level);

                return Random.Range(minNumber, maxNumber + 1);
            }
        }

        [Tooltip("This traitValue will determine the chance that enemy will drop items.")]
        [SerializeField]
        private float[] dropChancePercentage;

        [Tooltip("Min number of items that enemy can drop.")]
        [SerializeField]
        private int[] minDropsQuantity;

        [Tooltip("Max number of pickups that enemy can drop.")]
        [SerializeField]
        private int[] maxDropsQuantity;

        /// <summary>
        /// Return dropped items and their quantity.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public IEnumerable<Dropped> GetRandomDrops(int level)
        {
            if (!ShouldRandomDrop(level))
            {
                yield break;
            }

            for (int i = 0; i < GetRandomNumberOfDrops(level); i++)
            {
                yield return GetRandomDrop(level);
            }
        }

        public struct Dropped
        {
            public InventoryItemSO item;
            public int number;
        }

        /// <summary>
        /// Determine whether Enemy drops items or not
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private bool ShouldRandomDrop(int level)
        {
            return Random.Range(0, 100f) < GetByLevel(dropChancePercentage, level);
        }

        /// <summary>
        /// Return number of DropConfig that will be dropped.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private int GetRandomNumberOfDrops(int level)
        {
            int min = GetByLevel(minDropsQuantity, level);
            int max = GetByLevel(maxDropsQuantity, level);

            return Random.Range(min, max + 1);
        }

        private Dropped GetRandomDrop(int level)
        {
            DropConfig dropConfig = SelectRandomItem(level);
            Dropped drop = new Dropped();

            drop.item = dropConfig.item;
            drop.number = dropConfig.GetRandomNumber(level);

            return drop;
        }

        private DropConfig SelectRandomItem(int level)
        {
            float totalRelativeChance = GetTotalRelativeChance(level);
            float randomRoll = Random.Range(0, totalRelativeChance);
            float chanceTotal = 0;
            foreach (var drop in potentialDrops)
            {
                chanceTotal += GetByLevel(drop.relativeChances, level);
                if (chanceTotal > randomRoll)
                {
                    return drop;
                }
            }

            return null;
        }

        private float GetTotalRelativeChance(int level)
        {
            float total = 0;
            foreach (var drop in potentialDrops)
            {
                total += GetByLevel(drop.relativeChances, level);
            }

            return total;
        }

        private static T GetByLevel<T>(T[] values, int level)
        {
            if (values.Length == 0)
                return default;
            if (level > values.Length)
                return values[values.Length - 1];
            if (level < 1)
                return default;

            return values[level - 1];
        }
    }
}
