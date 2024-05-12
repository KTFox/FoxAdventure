using RPG.Stats;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class StatInfoUI : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private TextMeshProUGUI _value;
        [SerializeField]
        private Stat _stat;

        private BaseStats _playerBaseStat;


        // Methods

        private void Start()
        {
            _playerBaseStat = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            _value.text = _playerBaseStat.GetValueOfStat(_stat).ToString();
        }
    }
}
