using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class TraitStore : MonoBehaviour
    {
        private Dictionary<Trait, int> assignedPoints = new Dictionary<Trait, int>();
        private int _unAssignedPoints = 10;

        public int UnAssignedPoints => _unAssignedPoints;

        public void AssignPoints(Trait trait, int points)
        {
            if (!CanAssignTraits(trait, points)) return;

            assignedPoints[trait] = GetTraits(trait) + points;
            _unAssignedPoints -= points;
        }

        public bool CanAssignTraits(Trait trait, int points)
        {
            if (GetTraits(trait) + points < 0) return false;
            if (_unAssignedPoints < points) return false;

            return true;
        }

        public int GetTraits(Trait trait)
        {
            return assignedPoints.ContainsKey(trait) ? assignedPoints[trait] : 0;
        }
    }
}
