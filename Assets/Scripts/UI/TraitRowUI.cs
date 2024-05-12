using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RPG.Stats;

namespace RPG.UI
{
    public class TraitRowUI : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private Trait _trait;
        [SerializeField]
        private TextMeshProUGUI _traitValueText;
        [SerializeField]
        private Button _minusButton;
        [SerializeField]
        private Button _plusButton;

        private TraitStore _playerTraitStore;


        // Methods

        private void Start()
        {
            _playerTraitStore = GameObject.FindGameObjectWithTag("Player").GetComponent<TraitStore>();

            _minusButton.onClick.AddListener(() => AllocateTraitPoint(-1));
            _plusButton.onClick.AddListener(() => AllocateTraitPoint(1));
        }

        private void Update()
        {
            _minusButton.interactable = _playerTraitStore.CanAssignTraits(_trait, -1);
            _plusButton.interactable = _playerTraitStore.CanAssignTraits(_trait, 1);

            _traitValueText.text = _playerTraitStore.GetProposedPoints(_trait).ToString();
        }

        #region Unity Events
        public void AllocateTraitPoint(int value)
        {
            _playerTraitStore.AssignPoints(_trait, value);
        }
        #endregion
    }
}
