using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Abilities
{
    public class AbilityData : IAction
    {
        private GameObject _user;
        private Vector3 _targetedPoint;
        private IEnumerable<GameObject> _targets;
        private bool _cancelled;

        public AbilityData(GameObject user)
        {
            _user = user;
        }

        #region Properties
        public GameObject User => _user;
        public Vector3 TargetPoint => _targetedPoint;
        public IEnumerable<GameObject> Targets => _targets;
        public bool Cancelled => _cancelled;
        #endregion

        public void SetTargetedPoint(Vector3 targetedPoint)
        {
            _targetedPoint = targetedPoint;
        }

        public void SetTargets(IEnumerable<GameObject> targets)
        {
            _targets = targets;
        }

        public void StartCoroutine(IEnumerator coroutine)
        {
            _user.GetComponent<MonoBehaviour>().StartCoroutine(coroutine);
        }

        #region IAction implements
        public void Cancel()
        {
            _cancelled = true;
        }
        #endregion
    }
}
