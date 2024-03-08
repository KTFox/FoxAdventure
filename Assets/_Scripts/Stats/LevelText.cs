using System;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class LevelText : MonoBehaviour
    {
        private BaseStats playerBaseStats;
        private TextMeshProUGUI levelValueText;

        private void Awake()
        {
            playerBaseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
            levelValueText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            levelValueText.text = String.Format("Level: {0}", playerBaseStats.CurrentLevel);
        }
    }
}
