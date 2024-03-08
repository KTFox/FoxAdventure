using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthText : MonoBehaviour
    {
        private Health playerHealth;
        private TextMeshProUGUI healthValueText;

        private void Awake()
        {
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            healthValueText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            healthValueText.text = $"Health: {playerHealth.CurrentHealth}/{playerHealth.MaxHealth}";
        }
    }
}
