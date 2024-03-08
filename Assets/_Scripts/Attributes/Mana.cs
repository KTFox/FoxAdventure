using UnityEngine;
using RPG.Stats;
using RPG.Utility;

namespace RPG.Attributes
{
    public class Mana : MonoBehaviour
    {
        private LazyValue<float> _currentMana;

        #region Properties
        public float MaxMana
        {
            get => GetComponent<BaseStats>().GetStat(Stat.Mana);
        }

        public float CurrentMana
        {
            get => _currentMana.Value;
        }

        public float ManaRecover
        {
            get => GetComponent<BaseStats>().GetStat(Stat.ManaRecover);
        }
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
    }
}
