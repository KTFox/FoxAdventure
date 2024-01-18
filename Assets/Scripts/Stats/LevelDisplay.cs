using System;
using TMPro;
using UnityEngine;

namespace RPG.Stats {
    public class LevelDisplay : MonoBehaviour {

        private BaseStats playerBaseStats;
        private TextMeshProUGUI experienceValueText;

        private void Awake() {
            playerBaseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
            experienceValueText = GetComponent<TextMeshProUGUI>();
        }

        private void Update() {
            UpdateLevelValueText();
        }

        private void UpdateLevelValueText() {
            experienceValueText.text = String.Format("{0}", playerBaseStats.CalculateLevel());
        }
    }
}
