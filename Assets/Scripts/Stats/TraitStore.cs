using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Stats
{
    public class TraitStore : MonoBehaviour, IModifierProvider, ISaveable
    {
        // Structs

        [System.Serializable]
        private class TraitBonus
        {
            public Trait trait;
            public Stat stat;
            public float additiveBonusPerPoint;
            public float percentageBonusPerPoint;
        }

        // Variables

        [SerializeField]
        private TraitBonus[] _traitBonuses;
        private Dictionary<Trait, int> _assignedPoints = new Dictionary<Trait, int>();
        private Dictionary<Trait, int> _stagedPoints = new Dictionary<Trait, int>();
        private Dictionary<Stat, Dictionary<Trait, float>> _additiveBonusCache = new Dictionary<Stat, Dictionary<Trait, float>>();
        private Dictionary<Stat, Dictionary<Trait, float>> _percentageBonusCache = new Dictionary<Stat, Dictionary<Trait, float>>();

        // Properties

        public int UnAssignedPoints => AssignablePoints - GetTotalProposedPoints();
        public int AssignablePoints => (int)GetComponent<BaseStats>().GetValueOfStat(Stat.TotalTraitPoints);


        // Methods

        private void Awake()
        {
            foreach (TraitBonus traitBonus in _traitBonuses)
            {
                if (!_additiveBonusCache.ContainsKey(traitBonus.stat))
                {
                    _additiveBonusCache[traitBonus.stat] = new Dictionary<Trait, float>();
                }

                if (!_percentageBonusCache.ContainsKey(traitBonus.stat))
                {
                    _percentageBonusCache[traitBonus.stat] = new Dictionary<Trait, float>();
                }

                _additiveBonusCache[traitBonus.stat][traitBonus.trait] = traitBonus.additiveBonusPerPoint;
                _percentageBonusCache[traitBonus.stat][traitBonus.trait] = traitBonus.percentageBonusPerPoint;
            }
        }


        public void AssignPoints(Trait trait, int points)
        {
            if (!CanAssignTraits(trait, points)) return;

            _stagedPoints[trait] = GetStagedPoints(trait) + points;
        }

        public bool CanAssignTraits(Trait trait, int points)
        {
            if (GetStagedPoints(trait) + points < 0)
            {
                return false;
            }

            if (UnAssignedPoints < points)
            {
                return false;
            }

            return true;
        }

        public int GetTotalProposedPoints()
        {
            int total = 0;

            foreach (int point in _assignedPoints.Values)
            {
                total += point;
            }

            foreach (int point in _stagedPoints.Values)
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
            return _assignedPoints.ContainsKey(trait) ? _assignedPoints[trait] : 0;
        }

        public int GetStagedPoints(Trait trait)
        {
            return _stagedPoints.ContainsKey(trait) ? _stagedPoints[trait] : 0;
        }

        #region IModifierProvider implements
        IEnumerable<float> IModifierProvider.GetAdditiveModifiers(Stat stat)
        {
            if (!_additiveBonusCache.ContainsKey(stat)) yield break;

            foreach (Trait trait in _additiveBonusCache[stat].Keys)
            {
                yield return GetAssignedPoints(trait) * _additiveBonusCache[stat][trait];
            }
        }

        IEnumerable<float> IModifierProvider.GetPercentageModifiers(Stat stat)
        {
            if (!_percentageBonusCache.ContainsKey(stat)) yield break;

            foreach (Trait trait in _percentageBonusCache[stat].Keys)
            {
                yield return GetAssignedPoints(trait) * _percentageBonusCache[stat][trait];
            }
        }
        #endregion

        #region ISaveable implements
        object ISaveable.CaptureState()
        {
            return _assignedPoints;
        }

        void ISaveable.RestoreState(object state)
        {
            _assignedPoints = (Dictionary<Trait, int>)state;
        }
        #endregion

        #region Unity Events
        public void Commit()
        {
            foreach (var pair in _stagedPoints.Keys)
            {
                _assignedPoints[pair] = GetProposedPoints(pair);
            }
            _stagedPoints.Clear();
        }
        #endregion
    }
}
