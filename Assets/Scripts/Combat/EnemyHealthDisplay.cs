using RPG.Attributes;
using System;
using TMPro;
using UnityEngine;

namespace RPG.Combat {
    public class EnemyHealthDisplay : MonoBehaviour {

        private Fighter fighter;
        private TextMeshProUGUI healthValueText;

        private void Awake() {
            healthValueText = GetComponent<TextMeshProUGUI>();
            fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
        }

        private void Update() {
            UpdateHealthText();
        }

        private void UpdateHealthText() {
            Health targetHealth = fighter.GetTargetHealth();

            if (targetHealth != null) {
                healthValueText.text = String.Format("{0:0.0}%", targetHealth.GetHealthPercentage());
            } else {
                healthValueText.text = "N/A";
            }
        }
    }
}
