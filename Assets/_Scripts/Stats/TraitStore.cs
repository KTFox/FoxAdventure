using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Stats
{
    public class TraitStore : MonoBehaviour, IModifierProvider, ISaveable
    {
        [SerializeField]
        private TraitBonus[] bonusConfig;

        [System.Serializable]
        private class TraitBonus
        {
            public Trait trait;
            public Stat stat;
            public float additiveBonusPerPoint;
            public float percentageBonusPerPoint;
        }

        private Dictionary<Trait, int> assignedPoints = new Dictionary<Trait, int>();
        private Dictionary<Trait, int> stagedPoints = new Dictionary<Trait, int>();
        private Dictionary<Stat, Dictionary<Trait, float>> additiveBonusCache = new Dictionary<Stat, Dictionary<Trait, float>>();
        private Dictionary<Stat, Dictionary<Trait, float>> percentageBonusCache = new Dictionary<Stat, Dictionary<Trait, float>>();

        #region Properties
        public int UnAssignedPoints => AssignablePoints - GetTotalProposedPoints();
        public int AssignablePoints => (int)GetComponent<BaseStats>().GetStat(Stat.TotalTraitPoints);
        #endregion

        private void Awake()
        {
            foreach (var bonus in bonusConfig)
            {
                if (!additiveBonusCache.ContainsKey(bonus.stat))
                {
                    additiveBonusCache[bonus.stat] = new Dictionary<Trait, float>();
                }
                if (!percentageBonusCache.ContainsKey(bonus.stat))
                {
                    percentageBonusCache[bonus.stat] = new Dictionary<Trait, float>();
                }
                additiveBonusCache[bonus.stat][bonus.trait] = bonus.additiveBonusPerPoint;
                percentageBonusCache[bonus.stat][bonus.trait] = bonus.percentageBonusPerPoint;
            }
        }


        public void AssignPoints(Trait trait, int points)
        {
            if (!CanAssignTraits(trait, points)) return;

            stagedPoints[trait] = GetStagedPoints(trait) + points;
        }

        public bool CanAssignTraits(Trait trait, int points)
        {
            if (GetStagedPoints(trait) + points < 0) return false;
            if (UnAssignedPoints < points) return false;

            return true;
        }

        public int GetTotalProposedPoints()
        {
            int total = 0;
            foreach (int point in assignedPoints.Values)
            {
                total += point;
            }
            foreach (int point in stagedPoints.Values)
            {
                total += point;
            }

            return total;
        }

        public int GetProposedPoints(Trait trait)
        {
            return GetAssignedPoints(trait) + GetStagedPoints(trait);
        }

        public int GetAssignedPoints(Trait trait)
        {
            return assignedPoints.ContainsKey(trait) ? assignedPoints[trait] : 0;
        }

        public int GetStagedPoints(Trait trait)
        {
            return stagedPoints.ContainsKey(trait) ? stagedPoints[trait] : 0;
        }

        #region IModifierProvider implements
        IEnumerable<float> IModifierProvider.GetAdditiveModifiers(Stat stat)
        {
            if (!additiveBonusCache.ContainsKey(stat)) yield break;

            foreach (Trait trait in additiveBonusCache[stat].Keys)
            {
                yield return GetAssignedPoints(trait) * additiveBonusCache[stat][trait];
            }
        }

        IEnumerable<float> IModifierProvider.GetPercentageModifiers(Stat stat)
        {
            if (!percentageBonusCache.ContainsKey(stat)) yield break;

            foreach (Trait trait in percentageBonusCache[stat].Keys)
            {
                yield return GetAssignedPoints(trait) * percentageBonusCache[stat][trait];
            }
        }
        #endregion

        #region ISaveable implements
        object ISaveable.CaptureState()
        {
            return assignedPoints;
        }

        void ISaveable.RestoreState(object state)
        {
            assignedPoints = (Dictionary<Trait, int>)state;
        }
        #endregion

        #region Unity Events
        public void Commit()
        {
            foreach (var pair in stagedPoints.Keys)
            {
                assignedPoints[pair] = GetProposedPoints(pair);
            }
            stagedPoints.Clear();
        }
        #endregion
    }
}
