using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class TraitStore : MonoBehaviour
    {
        private Dictionary<Trait, int> assignedPoints = new Dictionary<Trait, int>();
        private Dictionary<Trait, int> stagedPoints = new Dictionary<Trait, int>();

        #region Properties
        public int UnAssignedPoints => AssignablePoints - GetTotalProposedPoints();
        public int AssignablePoints => (int)GetComponent<BaseStats>().GetStat(Stat.TotalTraitPoints);
        #endregion

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
            foreach(int point in assignedPoints.Values)
            {
                total += point;
            }
            foreach(int point in stagedPoints.Values)
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
