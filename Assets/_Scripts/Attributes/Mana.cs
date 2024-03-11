using UnityEngine;
using RPG.Stats;
using RPG.Utility;
using RPG.Saving;

namespace RPG.Attributes
{
    public class Mana : MonoBehaviour, ISaveable
    {
        private LazyValue<float> _currentMana;

        #region Properties
        public float MaxMana => GetComponent<BaseStats>().GetStat(Stat.Mana);
        public float CurrentMana => _currentMana.Value;
        public float ManaRecover => GetComponent<BaseStats>().GetStat(Stat.ManaRecover);
        #endregion

        private void Awake()
        {
            _currentMana = new LazyValue<float>(GetMaxMana);
        }

        float GetMaxMana()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Mana);
        }

        private void Update()
        {
            if (_currentMana.Value < GetMaxMana())
            {
                _currentMana.Value += ManaRecover * Time.deltaTime;

                if (_currentMana.Value > GetMaxMana())
                {
                    _currentMana.Value = GetMaxMana();
                }
            }
        }

        public bool UseMana(float manaToUse)
        {
            if (manaToUse > _currentMana.Value) return false;

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
