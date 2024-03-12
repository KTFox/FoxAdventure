using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RPG.Stats;

namespace RPG.UI
{
    public class TraitRowUI : MonoBehaviour
    {
        [SerializeField]
        private Trait trait;
        [SerializeField]
        private TextMeshProUGUI traitValue;
        [SerializeField]
        private Button minusButton;
        [SerializeField]
        private Button plusButton;

        private TraitStore playerTraitStore;

        private void Start()
        {
            playerTraitStore = GameObject.FindGameObjectWithTag("Player").GetComponent<TraitStore>();

            minusButton.onClick.AddListener(() => Allocate(-1));
            plusButton.onClick.AddListener(() => Allocate(1));
        }

        private void Update()
        {
            minusButton.interactable = playerTraitStore.CanAssignTraits(trait, -1);
            plusButton.interactable = playerTraitStore.CanAssignTraits(trait, 1);

            traitValue.text = playerTraitStore.GetTraits(trait).ToString();
        }

        public void Allocate(int value)
        {
            playerTraitStore.AssignPoints(trait, value);
        }
    }
}
