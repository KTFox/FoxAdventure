using System;
using TMPro;
using UnityEngine;

namespace RPG.Attributes {
    public class HealthDisplay : MonoBehaviour {

        private Health health;
        private TextMeshProUGUI healthText;

        private void Awake() {
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            healthText = GetComponent<TextMeshProUGUI>();
        }

        private void Update() {
            UpdateHealthText();
        }

        private void UpdateHealthText() {
            healthText.text = String.Format("{0:0.0}", health.GetHealthPercentage());         
        }
    }
}
