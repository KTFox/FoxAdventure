using System;
using TMPro;
using UnityEngine;

namespace RPG.Attributes {
    public class PlayerHealthDisplay : MonoBehaviour {

        private Health health;
        private TextMeshProUGUI healthValueText;

        private void Awake() {
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            healthValueText = GetComponent<TextMeshProUGUI>();
        }

        private void Update() {
            UpdateHealthText();
        }

        private void UpdateHealthText() {
            healthValueText.text = String.Format("{0:0.0}%", health.GetHealthPercentage());         
        }
    }
}
