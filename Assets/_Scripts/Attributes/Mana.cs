using UnityEngine;
using RPG.Stats;
using RPG.Utility;
using RPG.Saving;

namespace RPG.Attributes
{
    public class Mana : MonoBehaviour, ISaveable
    {
        // Variables

        private LazyValue<float> _currentMana;

        // Properties

        public float MaxMana => GetComponent<BaseStats>().GetValueOfStat(Stat.Mana);
        public float CurrentMana => _currentMana.Value;
        public float ManaRecover => GetComponent<BaseStats>().GetValueOfStat(Stat.ManaRecover);


        // Methods

        private void Awake()
        {
            _currentMana = new LazyValue<float>(GetInitialMana);
        }

        float GetInitialMana()
        {
            return GetComponent<BaseStats>().GetValueOfStat(Stat.Mana);
        }

        private void Update()
        {
            RestoreManaOverTime();
        }

        void RestoreManaOverTime()
        {
            if (_currentMana.Value < GetInitialMana())
            {
                _currentMana.Value += ManaRecover * Time.deltaTime;

                if (_currentMana.Value > GetInitialMana())
                {
                    _currentMana.Value = GetInitialMana();
                }
            }
        }

        public bool UseMana(float manaToUse)
        {
            if (manaToUse > _currentMana.Value)
            {
                return false;
            }

            _currentMana.Value -= manaToUse;

            return true;
        }

        #region ISaveable implements
        object ISaveable.CaptureState()
        {
            return _currentMana.Value;
        }

        void ISaveable.RestoreState(object state)
        {
            _currentMana.Value = (float)state;
        }
        #endregion
    }
}
