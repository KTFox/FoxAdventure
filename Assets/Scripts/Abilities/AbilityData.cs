using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Abilities
{
    public class AbilityData : IAction
    {
        // Variables

        private GameObject _instigator;
        private Vector3 _targetedPoint;
        private IEnumerable<GameObject> _targets;
        private bool _isCancelled;

        // Constructor

        public AbilityData(GameObject user)
        {
            _instigator = user;
        }

        // Properties

        public GameObject Instigator => _instigator;
        public Vector3 TargetPoint
        {
            get => _targetedPoint;
            set => _targetedPoint = value;
        }
        public IEnumerable<GameObject> Targets
        {
            get => _targets;
            set => _targets = value;
        }
        public bool IsCancelled => _isCancelled;


        // Methods

        public void StartCoroutine(IEnumerator coroutine)
        {
            _instigator.GetComponent<MonoBehaviour>().StartCoroutine(coroutine);
        }

        #region IAction implements
        public void Cancel()
        {
            _isCancelled = true;
        }
        #endregion
    }
}
