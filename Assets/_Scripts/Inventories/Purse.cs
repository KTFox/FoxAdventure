using System;
using UnityEngine;

namespace RPG.Inventories
{
    public class Purse : MonoBehaviour
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

        private void Start()
        {
            _currentBalance = startingBalance;

            Debug.Log($"Balance: {_currentBalance}");
        }

        public void UpdateBalance(float amount)
        {
            _currentBalance += amount;

            OnPurseUpdated?.Invoke();

            Debug.Log($"Balance: {_currentBalance}");
        }
    }
}
