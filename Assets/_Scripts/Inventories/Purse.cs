using RPG.Saving;
using System;
using UnityEngine;

namespace RPG.Inventories
{
    public class Purse : MonoBehaviour, ISaveable
    {
        public event Action OnPurseUpdated;

        [SerializeField]
        private float startingBalance = 50f;

        private float _currentBalance;

        #region Properties
        public float CurrentBalance
        {
            get => _currentBalance;
        }
        #endregion

        private void Awake()
        {
            _currentBalance = startingBalance;
        }

        public void UpdateBalance(float amount)
        {
            _currentBalance += amount;

            OnPurseUpdated?.Invoke();
        }

        #region ISaveable implements
        object ISaveable.CaptureState()
        {
            return _currentBalance;
        }

        void ISaveable.RestoreState(object state)
        {
            _currentBalance = (float)state;
        }
        #endregion
    }
}
