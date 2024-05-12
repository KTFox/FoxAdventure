using System;
using UnityEngine;
using RPG.Saving;

namespace RPG.Inventories
{
    public class Purse : MonoBehaviour, ISaveable, IItemStore
    {
        // Variables

        [SerializeField]
        private float _startingBalance = 50f;
        private float _currentBalance;

        // Properties

        public float CurrentBalance => _currentBalance;

        // Events

        public event Action OnPurseUpdated;


        // Methods

        private void Awake()
        {
            _currentBalance = _startingBalance;
        }

        public void UpdateBalance(float amount)
        {
            _currentBalance += amount;

            OnPurseUpdated?.Invoke();
        }

        #region IItemStore implements
        int IItemStore.AddItems(InventoryItemSO item, int number)
        {
            if (item is CurrencyItemSO)
            {
                UpdateBalance(item.Price * number);
                
                return number;
            }

            return 0;
        }
        #endregion

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
