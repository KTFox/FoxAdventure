using TMPro;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Combat {
    public class EnemyHealthText : MonoBehaviour {

        private Fighter fighter;
        private TextMeshProUGUI healthValueText;

        private void Awake() {
            healthValueText = GetComponent<TextMeshProUGUI>();
            fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
        }

        private void Update() {
            Health targetHealth = fighter.TargetHealth;

            if (targetHealth != null) {
                healthValueText.text = $"{targetHealth.CurrentHealth}/{targetHealth.MaxHealth}";
            } else {
                healthValueText.text = "N/A";
            }
        }
    }
}
