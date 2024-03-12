using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class TraitStore : MonoBehaviour
    {
        private Dictionary<Trait, int> assignedPoints = new Dictionary<Trait, int>();
        private Dictionary<Trait, int> stagedPoints = new Dictionary<Trait, int>();
        private int _unAssignedPoints = 10;

        public int UnAssignedPoints => _unAssignedPoints;

        public void AssignPoints(Trait trait, int points)
        {
            if (!CanAssignTraits(trait, points)) return;

            stagedPoints[trait] = GetStagedPoints(trait) + points;
            _unAssignedPoints -= points;
        }

        public bool CanAssignTraits(Trait trait, int points)
        {
            if (GetStagedPoints(trait) + points < 0) return false;
            if (_unAssignedPoints < points) return false;

            return true;
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
