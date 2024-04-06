using UnityEngine;
using RPG.Attributes;
using RPG.Stats;
using TMPro;

namespace RPG.UI
{
    public class PlayerHUD : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private RectTransform _healthFill;
        [SerializeField]
        private TextMeshProUGUI _healthText;
        [SerializeField]
        private RectTransform _manaFill;
        [SerializeField]
        private TextMeshProUGUI _manaText;
        [SerializeField]
        private RectTransform _exprimentFill;

        private Health _playerHealth;
        private Mana _playerMana;
        private Experience _playerExperience;


        // Methods

        private void Start()
        {
            _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            _playerMana = GameObject.FindGameObjectWithTag("Player").GetComponent<Mana>();
            _playerExperience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            _healthFill.localScale = new Vector3(_playerHealth.CurrentHealthFraction, _healthFill.localScale.y, _healthFill.localScale.z);
            _manaFill.localScale = new Vector3(_playerMana.CurrentManaFraction, _manaFill.localScale.y, _manaFill.localScale.z);

            _healthText.text = $"{_playerHealth.CurrentHealth:N0}/{_playerHealth.MaxHealth:N0}";
            _manaText.text = $"{_playerMana.CurrentMana:N0}/{_playerMana.MaxMana:N0}";
        }
    }
}
