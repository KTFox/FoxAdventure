using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class TraitUI : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private TextMeshProUGUI _availableTraitsText;
        [SerializeField]
        private Button _confirmButton;

        private TraitStore _playerTraitStore;


        // Methods

        private void Start()
        {
            _playerTraitStore = GameObject.FindGameObjectWithTag("Player").GetComponent<TraitStore>();

            _confirmButton.onClick.AddListener(() => _playerTraitStore.Commit());
        }

        private void Update()
        {
            _availableTraitsText.text = _playerTraitStore.UnAssignedPoints.ToString();
        }
    }
}
