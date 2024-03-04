using UnityEngine;

namespace RPG.Inventories
{
    public class Purse : MonoBehaviour
    {
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

            Debug.Log($"Balance: {_currentBalance}");
        }
    }
}
