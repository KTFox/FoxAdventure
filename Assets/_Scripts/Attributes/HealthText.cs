using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthText : MonoBehaviour
    {
        // Variables

        private Health playerHealth;
        private TextMeshProUGUI healthValueText;


        // Methods

        private void Awake()
        {
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            healthValueText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            healthValueText.text = $"Health: {playerHealth.CurrentHealth:N2}/{playerHealth.MaxHealth:N2}";
        }
    }
}
