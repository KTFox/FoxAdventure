using TMPro;
using UnityEngine;

namespace RPG.Attributes {
    public class PlayerHealthText : MonoBehaviour {

        private Health playerHealth;
        private TextMeshProUGUI healthValueText;

        private void Awake() {
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            healthValueText = GetComponent<TextMeshProUGUI>();
        }

        private void Update() {
            healthValueText.text = $"{playerHealth.CurrentHealth}/{playerHealth.MaxHealth}";
        }
    }
}
