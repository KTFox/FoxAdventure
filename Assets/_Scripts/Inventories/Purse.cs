using System;
using UnityEngine;
using RPG.Saving;

namespace RPG.Inventories
{
    public class Purse : MonoBehaviour, ISaveable, IItemStore
    {
        public event Action OnPurseUpdated;

        [SerializeField]
        private float startingBalance = 50f;

        private float _currentBalance;

        public float CurrentBalance => _currentBalance;

        private void Awake()
        {
            _currentBalance = startingBalance;
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
