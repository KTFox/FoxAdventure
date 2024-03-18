using System;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class LevelText : MonoBehaviour
    {
        // Variables

        private BaseStats _playerBaseStats;
        private TextMeshProUGUI _levelValueText;


        // Methods

        private void Awake()
        {
            _playerBaseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
            _levelValueText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            _levelValueText.text = String.Format("Level: {0}", _playerBaseStats.CurrentLevel);
        }
    }
}
