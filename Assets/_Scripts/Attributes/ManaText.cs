using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class ManaText : MonoBehaviour
    {
        // Variables

        private Mana playerMana;
        private TextMeshProUGUI manaValueText;


        // Methods

        private void Awake()
        {
            playerMana = GameObject.FindGameObjectWithTag("Player").GetComponent<Mana>();
            manaValueText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            manaValueText.text = $"Mana: {playerMana.CurrentMana:N0}/{playerMana.MaxMana}";
        }
    }
}
