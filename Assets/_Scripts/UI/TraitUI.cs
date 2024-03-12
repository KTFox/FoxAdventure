using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class TraitUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI availableTraitText;
        [SerializeField]
        private Button confirmButton;

        private TraitStore playerTraitStore;

        private void Start()
        {
            playerTraitStore = GameObject.FindGameObjectWithTag("Player").GetComponent<TraitStore>();

            confirmButton.onClick.AddListener(() => playerTraitStore.Commit());
        }

        private void Update()
        {
            availableTraitText.text = playerTraitStore.UnAssignedPoints.ToString();
        }
    }
}
