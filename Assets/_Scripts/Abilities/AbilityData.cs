using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities
{
    public class AbilityData
    {
        private GameObject _user;
        private IEnumerable<GameObject> _targets;

        public AbilityData(GameObject user)
        {
            _user = user;
        }

        #region Properties
        public GameObject User
        {
            get => _user;
        }

        public IEnumerable<GameObject> Targets
        {
            get => _targets;
        }
        #endregion

        public void SetTargets(IEnumerable<GameObject> targets)
        {
            _targets = targets;
        }
    }
}
