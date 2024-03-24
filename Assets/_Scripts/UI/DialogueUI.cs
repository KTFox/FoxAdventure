using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RPG.Dialogues;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private TextMeshProUGUI _AIText;
        [SerializeField]
        private Button _nextButton;

        private PlayerConversant _playerConversant;


        // Methods

        private void Start()
        {
            _playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            _nextButton.onClick.AddListener(NextButton_OnClick);

            UpdateUI();
        }

        private void NextButton_OnClick()
        {
            _playerConversant.MoveToNextDialogueNode();
            UpdateUI();
        }

        private void UpdateUI()
        {
            _AIText.text = _playerConversant.GetCurrentDialogueText();
            _nextButton.gameObject.SetActive(_playerConversant.HasNextDialogueNode());
        }
    }
}
